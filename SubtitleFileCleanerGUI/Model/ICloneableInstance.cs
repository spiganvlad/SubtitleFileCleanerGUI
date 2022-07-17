namespace SubtitleFileCleanerGUI.Model
{
    // Interface describes an object that can create a clone of the T entity
    // T is usually an object that implements an interface
    public interface ICloneableInstance<T>
    {
        public T Clone();
    }
}
