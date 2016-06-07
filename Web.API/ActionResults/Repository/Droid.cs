using System.Collections.Generic;

namespace ActionResults.Repository
{
    public class Droid
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductSeries { get; set; }
        public List<string> Armaments { get; set; }
    }
}
