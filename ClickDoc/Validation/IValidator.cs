namespace ClickDoc.Validation
{
    interface IValidator<T>
    {
        bool Validate(T value);
    }
}
