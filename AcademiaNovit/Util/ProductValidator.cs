namespace AcademiaNovit.Utils;

public static class ProductValidator
{
    public static bool IsValidName(string name)
    {
        // Valida que el nombre no sea nulo, vacío o demasiado corto
        return !string.IsNullOrWhiteSpace(name) && name.Length >= 3;
    }

    public static bool IsValidPrice(decimal price)
    {
        // Valida que el precio sea mayor que 0
        return price > 0;
    }

    public static (bool IsValid, string ErrorMessage) ValidateProduct(string name, decimal price)
    {
        if (!IsValidName(name))
        {
            return (false, "El nombre del producto debe tener al menos 3 caracteres y no puede estar vacío.");
        }

        if (!IsValidPrice(price))
        {
            return (false, "El precio del producto debe ser mayor que 0.");
        }

        return (true, string.Empty);
    }
}