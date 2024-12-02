using Confluent.Kafka;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KafkaRuleEngine
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder(args)
				.ConfigureServices((context, services) =>
				{
					// Register rule engines
					services.AddSingleton<TradeRuleEngine>();
					services.AddSingleton<BidRuleEngine>();

					// Register consumers with appropriate rule engine dependencies
					services.AddSingleton<TradeConsumer>(sp =>
						new TradeConsumer(sp.GetRequiredService<TradeRuleEngine>()));
					services.AddSingleton<BidConsumer>(sp =>
						new BidConsumer(sp.GetRequiredService<BidRuleEngine>()));

					// Register keyed consumers
					services.AddKeyedSingleton<IKafkaConsumer, TradeConsumer>("TradeConsumer");
					services.AddKeyedSingleton<IKafkaConsumer, BidConsumer>("BidConsumer");
				})
				.Build();

			var serviceProvider = host.Services;

			// Start both consumers in parallel
			var tradeConsumer = serviceProvider.GetKeyedService<IKafkaConsumer>("TradeConsumer");
			var bidConsumer = serviceProvider.GetKeyedService<IKafkaConsumer>("BidConsumer");

			var tradeTask = Task.Run(() => tradeConsumer.ConsumeAsync());
			var bidTask = Task.Run(() => bidConsumer.ConsumeAsync());

			Console.WriteLine("Press any key to stop consumers...");
			Console.ReadKey();

			// Optional: Handle cancellation or cleanup if needed
			await Task.WhenAll(tradeTask, bidTask);
		}
	}

	public class TradeConsumer : IKafkaConsumer
	{
		private readonly string _topic = "trade";
		private readonly IRuleEngine _ruleEngine;

		public TradeConsumer(TradeRuleEngine ruleEngine)
		{
			_ruleEngine = ruleEngine;
		}

		public async Task ConsumeAsync()
		{
			var config = new ConsumerConfig
			{
				GroupId = "trade-consumer-group",
				BootstrapServers = "localhost:9092",
				AutoOffsetReset = AutoOffsetReset.Earliest
			};

			using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
			consumer.Subscribe(_topic);

			while (true)
			{
				var consumeResult = consumer.Consume();
				Console.WriteLine($"TradeConsumer received: {consumeResult.Message.Value}");
				_ruleEngine.ExecuteRules(consumeResult.Message.Value);
				await Task.Delay(100);
			}
		}
	}

	public class BidConsumer : IKafkaConsumer
	{
		private readonly string _topic = "Bid";
		private readonly IRuleEngine _ruleEngine;

		public BidConsumer(BidRuleEngine ruleEngine)
		{
			_ruleEngine = ruleEngine;
		}

		public async Task ConsumeAsync()
		{
			var config = new ConsumerConfig
			{
				GroupId = "bid-consumer-group",
				BootstrapServers = "localhost:9092",
				AutoOffsetReset = AutoOffsetReset.Earliest
			};

			using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
			consumer.Subscribe(_topic);

			while (true)
			{
				var consumeResult = consumer.Consume();
				Console.WriteLine($"BidConsumer received: {consumeResult.Message.Value}");
				_ruleEngine.ExecuteRules(consumeResult.Message.Value);
				await Task.Delay(100);
			}
		}
	}

	public interface IKafkaConsumer
	{
		Task ConsumeAsync();
	}

	public interface IRuleEngine
	{
		void ExecuteRules(string message);
	}

	public class BidRuleEngine : IRuleEngine
	{
		public void ExecuteRules(string message)
		{
			Console.WriteLine($"BidRuleEngine applied rules on message: {message}");
			// Add specific bid rules logic here
		}
	}

	public class TradeRuleEngine : IRuleEngine
	{
		public void ExecuteRules(string message)
		{
			Console.WriteLine($"TradeRuleEngine applied rules on message: {message}");
			// Add specific trade rules logic here
		}
	}
}
