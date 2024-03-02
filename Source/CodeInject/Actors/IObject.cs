
namespace CodeInject.Actors
{
    public unsafe interface IObject
    {
         long ObjectPointer { get; set; }
         ushort ID { get; set; }
         float X { get; set; }
         float Y { get; set; }
         float Z { get; set; }

         double CalcDistance(IObject actor);
         double CalcDistance(float x, float y, float z);
    }
}
