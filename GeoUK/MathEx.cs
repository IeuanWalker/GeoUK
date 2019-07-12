using System;

namespace GeoUK
{
    public static class MathEx
    {
        public static double Secant(double x) => 1 / Math.Cos(x);

        //			Cosecant Cosec(X) = 1 / Sin(X)
        //			Cotangent Cotan(X) = 1 / Tan(X)
        //			Inverse Sine Arcsin(X) = Atn(X / Sqr(-X * X + 1))
        //			Inverse Cosine Arccos(X) = Atn(-X / Sqr(-X * X + 1)) + 2 * Atn(1)
        //			Inverse Secant Arcsec(X) = 2 * Atn(1) - Atn(Sgn(X) / Sqr(X * X - 1))
        //			Inverse Cosecant Arccosec(X) = Atn(Sgn(X) / Sqr(X * X - 1))
        //			Inverse Cotangent Arccotan(X) = 2 * Atn(1) - Atn(X)
        //			Hyperbolic Sine HSin(X) = (Exp(X) - Exp(-X)) / 2
        //			Hyperbolic Cosine HCos(X) = (Exp(X) + Exp(-X)) / 2
        //			Hyperbolic Tangent HTan(X) = (Exp(X) - Exp(-X)) / (Exp(X) + Exp(-X))
        //			Hyperbolic Secant HSec(X) = 2 / (Exp(X) + Exp(-X))
        //			Hyperbolic Cosecant HCosec(X) = 2 / (Exp(X) - Exp(-X))
        //			Hyperbolic Cotangent HCotan(X) = (Exp(X) + Exp(-X)) / (Exp(X) - Exp(-X))
        //			Inverse Hyperbolic Sine HArcsin(X) = Log(X + Sqr(X * X + 1))
        //			Inverse Hyperbolic Cosine HArccos(X) = Log(X + Sqr(X * X - 1))
        //			Inverse Hyperbolic Tangent HArctan(X) = Log((1 + X) / (1 - X)) / 2
        //			Inverse Hyperbolic Secant HArcsec(X) = Log((Sqr(-X * X + 1) + 1) / X)
        //			Inverse Hyperbolic Cosecant HArccosec(X) = Log((Sgn(X) * Sqr(X * X + 1) + 1) / X)
        //			Inverse Hyperbolic Cotangent HArccotan(X) = Log((X + 1) / (X - 1)) / 2
        //			Logarithm to base N LogN(X) = Log(X) / Log(N)
    }
}