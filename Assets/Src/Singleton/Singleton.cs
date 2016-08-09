public class Singleton<T> where T : class,new()
{
    private static T instanse;
    public static T Instanse
    {
        get
        {
            if (instanse == null)
            {
                instanse = new T();
            }
            return instanse as T;
        }
    }
}
