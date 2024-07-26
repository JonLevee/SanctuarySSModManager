using NLua;
using SanctuarySSLib.Attributes;
using SanctuarySSLib.LuaUtil;
using SanctuarySSModManager.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SanctuarySSLib.MiscUtil
{
    [SingletonService]
    public class LuaObjectLoader
    {
        private readonly LuaObjectLoadParentHandler loadParentHandler;
        private readonly LuaObjectLoadObjectHandler loadObjectHandler;

        public LuaObjectLoader(
            LuaObjectLoadParentHandler loadParentHandler,
            LuaObjectLoadObjectHandler loadObjectHandler)
        {
            this.loadParentHandler = loadParentHandler;
            this.loadObjectHandler = loadObjectHandler;

        }


        public void Reload<T>(T instance) where T : class
        {

            loadParentHandler.Load(instance);
        }

        public T Load<T>() where T : class, new()
        {
            var instance = Activator.CreateInstance<T>();
            Reload<T>(instance);
            return instance;
        }



    }
}