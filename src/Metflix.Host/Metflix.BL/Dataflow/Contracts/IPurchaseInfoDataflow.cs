using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.KafkaModels;

namespace Metflix.BL.Dataflow.Contracts
{
    public interface IPurchaseInfoDataflow : IDataflow<PurchaseInfoData>
    {
    }
}
