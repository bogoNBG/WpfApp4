using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4.Model
{
    internal static class IdGenerator
    {
        private static Dictionary<Type, int> nextIds = new();

        public static int GetNextId<T>()
        {
            Type classType = typeof(T);

            if (!nextIds.ContainsKey(classType))
                nextIds[classType] = 1;

            int nextId = nextIds[classType];
            nextIds[classType]++;

            return nextId;
        }
    }
}
