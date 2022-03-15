using System;

namespace Utilities.DrawerFactory
{
    public abstract class ABaseDrawer
    {
        public abstract Type HandledType
        {
            get;
        }
    }
}