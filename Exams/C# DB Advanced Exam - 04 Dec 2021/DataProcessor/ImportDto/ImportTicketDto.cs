using System.ComponentModel.DataAnnotations;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTicketDto
    {
        [Range(typeof(decimal),"1.00", "100.00")]
        public decimal Price { get; set; }
        [Range(GlobalConstants.RowNumberMinValue, GlobalConstants.RowNumberMaxValue)]
        public sbyte RowNumber { get; set; }
        public int PlayId { get; set; }
    }
}
