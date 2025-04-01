using Confluent.Kafka;
namespace FirstProducer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private async void btnPublish_Click(object sender, EventArgs e)
		{
			// Kafka configuration
			var config = new ProducerConfig
			{
				BootstrapServers = "localhost:9092" // Replace with your Kafka broker address
			};

			// Topic name
			string topic = txtTopic.Text.Trim(); // Replace with your Kafka topic name

			// Create a producer
			using (var producer = new ProducerBuilder<string, string>(config).Build())
			{
				try
				{
					// Message to send
					string message = "Hello, Kafka!";

					// Add headers
					var headers = new Headers
				{
					{ "header-key-1", System.Text.Encoding.UTF8.GetBytes("header-value-1") },
					{ "header-key-2", System.Text.Encoding.UTF8.GetBytes("header-value-2") }
				};


					for (int i = 0; i < 10; i++)
					{
						var Key = chkUnique.Checked ? Guid.NewGuid().ToString() : "Key123";
						// Publish the message
						var dr = await producer.ProduceAsync(topic, new Message<string, string>
						{
							Key = Key,
							Value = $"{i} Message",
							Headers = headers
						});
						Console.WriteLine($"Message delivered to Partition: {dr.Partition} and Offset: {dr.Offset} ");
					}


					MessageBox.Show("Published Messages");

				}
				catch (ProduceException<string, string> ex)
				{
					Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
				}
			}

		}
	}
}
