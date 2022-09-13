using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Kafka
{
    public static class KafkaCloudConfig
    {
        public static Dictionary<string, string> GetConfig()
        {
            return new Dictionary<string, string>
            {
                {"bootstrap.servers","pkc-6ojv2.us-west4.gcp.confluent.cloud:9092"},
                {"security.protocol","SASL_SSL"},
                {"sasl.mechanisms","PLAIN"},
                {"sasl.username","27ZBRSXRGR2SUMP3"},
                {"sasl.password","JQNdk1jLvVKifTXQ/R3TlRvflHd3I30up+C/KT+wb6KX4Czs96hsm51ziaG/rOBY"},
                {"session.timeout.ms","45000"},
            };
        }
    }
}
