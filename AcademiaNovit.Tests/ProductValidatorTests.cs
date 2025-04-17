using AcademiaNovit.Utils;
using Xunit;

public class ProductValidatorTests
{
    [Theory]
    [InlineData("ValidName", true)] // Nombre válido
    [InlineData("No", false)]      // Nombre demasiado corto
    [InlineData("", false)]        // Nombre vacío
    public void IsValidName_ValidatesCorrectly(string name, bool expected)
    {
        // Act
        var result = ProductValidator.IsValidName(name);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(10.0, true)]  // Precio válido
    [InlineData(0.0, false)]  // Precio igual a 0
    [InlineData(-5.0, false)] // Precio negativo
    public void IsValidPrice_ValidatesCorrectly(decimal price, bool expected)
    {
        // Act
        var result = ProductValidator.IsValidPrice(price);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("ValidName", 10.0, true, "")] // Producto válido
    [InlineData("", 10.0, false, "El nombre del producto debe tener al menos 3 caracteres y no puede estar vacío.")]
    [InlineData("ValidName", 0.0, false, "El precio del producto debe ser mayor que 0.")]
    [InlineData("", 0.0, false, "El nombre del producto debe tener al menos 3 caracteres y no puede estar vacío.")]
    public void ValidateProduct_ValidatesCorrectly(string name, decimal price, bool expectedIsValid, string expectedErrorMessage)
    {
        // Act
        var (isValid, errorMessage) = ProductValidator.ValidateProduct(name, price);

        // Assert
        Assert.Equal(expectedIsValid, isValid);
        Assert.Equal(expectedErrorMessage, errorMessage);
    }
}