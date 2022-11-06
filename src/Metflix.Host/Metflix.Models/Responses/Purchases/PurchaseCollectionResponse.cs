using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.Responses.Purchases.PurchaseDtos;

namespace Metflix.Models.Responses.Purchases
{
    public class PurchaseCollectionResponse : BaseResponse<IEnumerable<PurchaseDto>>
    {
    }
}
