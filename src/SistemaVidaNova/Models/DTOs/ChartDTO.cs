using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.DTOs
{
    public class ChartDTO
    {
        public List<SerieDTO> Data = new List<SerieDTO>();
    }

    public class SerieDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public List<IDataPoint> datapoints { get; set; }
    }

    public interface IDataPoint
    {
    }

    public class DataPointString: IDataPoint
    {
        public string x { get; set; }
        public double y { get; set; }
    }

    public class DataPointDateTime : IDataPoint
    {
        public DateTime x { get; set; }
        public double y { get; set; }
    }
    public class DataPointFloat: IDataPoint
    {
        public double x { get; set; }
        public double y { get; set; }
    }


    public class DataPointLong: IDataPoint
    {
        public long x { get; set; }
        public double y { get; set; }
    }

}
