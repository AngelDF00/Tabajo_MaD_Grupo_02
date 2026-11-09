using System;
using System.Collections.Generic;


namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES
{
    public class Block<T>
    {
        public Boolean existMoreItems { get; set; }
        public IList<T> items { get; set; }

        public Block(Boolean existMoreItems, IList<T> items)
        {
            this.existMoreItems = existMoreItems;
            this.items = items;
        }

    }
}
