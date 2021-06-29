using System.ComponentModel.DataAnnotations;

namespace TestForItMonitoring.DatabaseModels
{
    public class RequestModel
    {
        [Key]
        public int Id { get; set; }
        public string ClientIp { get; set; }
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }
        public int Result { get; set; }
    }
}
