using System;
using System.Collections.Generic;
using System.Text;

namespace Util.DiagramDesigner
{
    public enum LogicalType
    {
        Input = -4,
        Output = -3,
        Constant = -2,
        Time = -1,
        None = 0,
        ADD = 1,
        SUB = 2,
        MUL = 3,
        DIV = 4,
        AVE = 5,
        MOD = 6,
        AND = 7,
        OR = 8,
        XOR = 9,
        NOT = 10,
        SHL = 11,//左移
        SHR = 12,//又移
        ROL = 13,//循环左移
        ROR = 14,//循环右移
        SEL = 15,//选择
        MAX = 16,
        MIN = 17,
        LIMIT = 18,//限幅
        GT = 19,//大于
        LT = 20,//小于
        GE = 21,//大于等于
        LE = 22,//小于等于
        EQ = 23,//等于
        NE = 24,//不等于
        ABS = 25,
        SQRT = 26,
        LN = 27,
        LOG = 28,
        EXP = 29,
        SIN = 30,
        COS = 31,
        TAN = 32,
        ASIN = 33,
        ACOS = 34,
        ATAN = 35,
        EXPT = 36,//幂
        Bool2Double = 37,
        Double2Bool = 38,
        BoolsToDouble = 39,
        DoubleToBools = 40,

    }
}
