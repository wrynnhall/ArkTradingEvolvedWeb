using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public enum CollectionForm
    {
        Add,
        Edit,
        View
    }

    public enum CollectionEntryForm {
        Add,
        Edit,
        View
    }

    public enum MarketEntryState
    {
        Available,
        Sold,
        Complete,
        Closed
    }

    public enum MarketEntryForm
    {
        Add,
        Edit,
        View
    }

    public enum CreatureForm
    {
        Add,
        Edit,
        View
    }

    public enum CreatureTypeForm
    {
        Add,
        Edit
    }

    public enum CreatureDietForm
    {
        Add,
        Edit
    }
}
