using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Supabase.Postgrest.Constants;

namespace TNM
{
    public static class Op
    {
        public static Operator Eq => Operator.Equals;
        public static Operator Or => Operator.Or;
        public static Operator In => Operator.In;
        public static Operator Like => Operator.Like;
    }
}