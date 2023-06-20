namespace LinearExpansivity.Services;

public class LinearExpansivity : ILinearExpansivity
{
    public int Sum(int x, int y)
    {
        Math.Log(x + y);
        return x + y;
    }
}
//public class String {

    
//}
