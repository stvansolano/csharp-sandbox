https://docs.microsoft.com/es-mx/dotnet/csharp/whats-new/csharp-8

# Novedades de C# 8.0

C# 8.0 agrega las siguientes características y mejoras al lenguaje C#:

- [Miembros de solo lectura](#readonly-members)
- [Métodos de interfaz predeterminados](#default-interface-methods)
- [Mejoras de coincidencia de patrones](#more-patterns-in-more-places):
  - [Expresiones switch](#switch-expressions)
  - [Patrones de propiedades](#property-patterns)
  - [Patrones de tupla](#tuple-patterns)
  - [Patrones posicionales](#positional-patterns)
- [Declaraciones using](#using-declarations)
- [Funciones locales estáticas](#static-local-functions)
- [Estructuras ref descartables](#disposable-ref-structs)
- [Tipos de referencia que aceptan valores null](#nullable-reference-types)
- [Secuencias asincrónicas](#asynchronous-streams)
- [Índices y rangos](#indices-and-ranges)
- [Asignación de uso combinado de NULL](#null-coalescing-assignment)
- [Tipos construidos no administrados](#unmanaged-constructed-types)
- [Stackalloc en expresiones anidadas](#stackalloc-in-nested-expressions)
- [Mejora de las cadenas textuales interpoladas](#enhancement-of-interpolated-verbatim-strings)

C# 8.0 se admite en **.NET Core 3.x** y **.NET Standard 2.1**. Para obtener más información, vea [Control de versiones del lenguaje C#](../language-reference/configure-language-version.md).

En el resto de este artículo se describen brevemente estas características. Cuando hay disponibles artículos detallados, se proporcionan vínculos a esos tutoriales e introducciones. Puede explorar estas características en su entorno mediante la herramienta global `dotnet try`:

1. Instale la herramienta global [dotnet-try](https://github.com/dotnet/try/blob/master/README.md#setup).
1. Clone el repositorio [dotnet/try-samples](https://github.com/dotnet/try-samples).
1. Establezca el directorio actual en el subdirectorio *csharp8* para el repositorio *try-samples*.
1. Ejecute `dotnet try`.

## Miembros de solo lectura (Readonly-members)

Puede aplicar el modificador `readonly` a los miembros de una estructura. Indica que el miembro no modifica el estado. Resulta más pormenorizado que aplicar el modificador `readonly` a una declaración `struct`.  Tenga en cuenta el siguiente struct mutable:

```csharp
public struct Point
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Distance => Math.Sqrt(X * X + Y * Y);

    public override string ToString() =>
        $"({X}, {Y}) is {Distance} from the origin";
}
```

Al igual que la mayoría de las estructuras, el método `ToString()` no modifica el estado. Para indicar eso, podría agregar el modificador `readonly` a la declaración de `ToString()`:

```csharp
public readonly override string ToString() =>
    $"({X}, {Y}) is {Distance} from the origin";
```

El cambio anterior genera una advertencia del compilador, porque `ToString` accede a la propiedad `Distance`, que no está marcada como `readonly`:

```console
warning CS8656: Call to non-readonly member 'Point.Distance.get' from a 'readonly' member results in an implicit copy of 'this'
```

El compilador le advierte cuando es necesario crear una copia defensiva.  La propiedad `Distance` no cambia el estado, por lo que puede corregir esta advertencia si agrega el modificador `readonly` a la declaración:

```csharp
public readonly double Distance => Math.Sqrt(X * X + Y * Y);
```

Tenga en cuenta que el modificador `readonly` es necesario en una propiedad de solo lectura. El compilador no presupone que los descriptores de acceso `get` no modifican el estado; debe declarar `readonly` explícitamente. Las propiedades implementadas automáticamente son una excepción; el compilador tratará todos los captadores implementados automáticamente como de solo lectura, por lo que aquí no es necesario agregar el modificador `readonly` a las propiedades `X` y `Y`.

El compilador aplica la regla por la que los miembros `readonly` no modifican el estado. El método siguiente no se compilará a menos que se quite el modificador `readonly`:

```csharp
public readonly void Translate(int xOffset, int yOffset)
{
    X += xOffset;
    Y += yOffset;
}
```

Esta característica permite especificar la intención del diseño para que el compilador pueda aplicarla, y realizar optimizaciones basadas en dicha intención. Puede obtener más información sobre los miembros de solo lectura en el artículo de referencia de lenguaje en [`readonly`](../language-reference/keywords/readonly.md#readonly-member-examples).

## Métodos de interfaz predeterminados (Default interface methods)

Ahora puede agregar miembros a interfaces y proporcionar una implementación de esos miembros. Esta característica del lenguaje permite que los creadores de API agreguen métodos a una interfaz en versiones posteriores sin interrumpir la compatibilidad binaria o de origen con implementaciones existentes de dicha interfaz. Las implementaciones existentes *heredan* la implementación predeterminada. Esta característica también permite que C# interopere con las API que tienen como destino Android o Swift, que admiten características similares. Los métodos de interfaz predeterminados también permiten escenarios similares a una característica del lenguaje de "rasgos".

Los métodos de interfaz predeterminados afectan a muchos escenarios y elementos del lenguaje. Nuestro primer tutorial abarca la [actualización de una interfaz con implementaciones predeterminadas](../tutorials/default-interface-methods-versions.md). Otros tutoriales y actualizaciones de referencia se incorporarán a tiempo en la versión general.

## Más patrones en más lugares (Pattern matching)

La **coincidencia de patrones** ofrece herramientas que proporcionan funcionalidad dependiente de la forma entre tipos de datos relacionados pero diferentes. C# 7.0 introdujo la sintaxis de patrones de tipos y patrones de constantes mediante la expresión [`is`](../language-reference/keywords/is.md) y la instrucción [`switch`](../language-reference/keywords/switch.md). Estas características representaban los primeros pasos hacia el uso de paradigmas de programación donde los datos y la funcionalidad están separados. A medida que el sector se mueve hacia más microservicios y otras arquitecturas basadas en la nube, se necesitan otras herramientas de lenguaje.

C# 8.0 amplía este vocabulario para que pueda usar más expresiones de patrones en más lugares del código. Cuando los datos y la funcionalidad estén separados, tenga en cuenta estas características. Cuando los algoritmos dependan de un hecho distinto del tipo de entorno de ejecución de un objeto, tenga en cuenta la coincidencia de patrones. Estas técnicas ofrecen otra forma de expresar los diseños.

Además de nuevos patrones en nuevos lugares C# 8.0 agrega **patrones recursivos**. El resultado de cualquier expresión de patrón es una expresión. Un patrón recursivo es simplemente una expresión de patrón aplicada a la salida de otra expresión de patrón.

### Expresiones switch

Con frecuencia, una instrucción [`switch`](../language-reference/keywords/switch.md) genera un valor en cada uno de sus bloques `case`. Las **expresiones switch** le permiten usar una sintaxis de expresiones más concisa. Hay menos palabras clave `case` y `break` repetitivas y menos llaves.  Por ejemplo, considere la siguiente enumeración que muestra los colores del arco iris:

```csharp
public enum Rainbow
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Indigo,
    Violet
}
```

Si la aplicación definió un tipo `RGBColor` construido a partir de los componentes `R`, `G` y `B`, podría convertir un valor `Rainbow` a sus valores RGB con el método siguiente que contiene una expresión switch:

```csharp
public static RGBColor FromRainbow(Rainbow colorBand) =>
    colorBand switch
    {
        Rainbow.Red    => new RGBColor(0xFF, 0x00, 0x00),
        Rainbow.Orange => new RGBColor(0xFF, 0x7F, 0x00),
        Rainbow.Yellow => new RGBColor(0xFF, 0xFF, 0x00),
        Rainbow.Green  => new RGBColor(0x00, 0xFF, 0x00),
        Rainbow.Blue   => new RGBColor(0x00, 0x00, 0xFF),
        Rainbow.Indigo => new RGBColor(0x4B, 0x00, 0x82),
        Rainbow.Violet => new RGBColor(0x94, 0x00, 0xD3),
        _              => throw new ArgumentException(message: "invalid enum value", paramName: nameof(colorBand)),
    };
```

Algunas mejoras en la sintaxis:

- La variable precede a la palabra clave `switch`. El orden diferente permite distinguir fácilmente la expresión switch de la instrucción switch.
- Los elementos `case` y `:` se reemplazan por `=>`. Es más concisa e intuitiva.
- El caso `default` se reemplaza por un descarte `_`.
- Los cuerpos son expresiones, no instrucciones.

Compárelo con el código equivalente mediante la instrucción clásica `switch`:

```csharp
public static RGBColor FromRainbowClassic(Rainbow colorBand)
{
    switch (colorBand)
    {
        case Rainbow.Red:
            return new RGBColor(0xFF, 0x00, 0x00);
        case Rainbow.Orange:
            return new RGBColor(0xFF, 0x7F, 0x00);
        case Rainbow.Yellow:
            return new RGBColor(0xFF, 0xFF, 0x00);
        case Rainbow.Green:
            return new RGBColor(0x00, 0xFF, 0x00);
        case Rainbow.Blue:
            return new RGBColor(0x00, 0x00, 0xFF);
        case Rainbow.Indigo:
            return new RGBColor(0x4B, 0x00, 0x82);
        case Rainbow.Violet:
            return new RGBColor(0x94, 0x00, 0xD3);
        default:
            throw new ArgumentException(message: "invalid enum value", paramName: nameof(colorBand));
    };
}
```

### Patrones de propiedades (Property Patterns)

El **patrón de propiedades** permite hallar la coincidencia con las propiedades del objeto examinado. Considere un sitio de comercio electrónico que debe calcular los impuestos de ventas según la dirección del comprador. Ese cálculo no es una responsabilidad fundamental de una clase `Address`. Cambiará con el tiempo, probablemente con más frecuencia que el formato de dirección. El importe de los impuestos de ventas depende de la propiedad `State` de la dirección. El método siguiente usa el patrón de propiedades para calcular los impuestos de ventas a partir de la dirección y el precio:

```csharp
public static decimal ComputeSalesTax(Address location, decimal salePrice) =>
    location switch
    {
        { State: "WA" } => salePrice * 0.06M,
        { State: "MN" } => salePrice * 0.75M,
        { State: "MI" } => salePrice * 0.05M,
        // other cases removed for brevity...
        _ => 0M
    };
```

La coincidencia de patrones crea una sintaxis concisa para expresar este algoritmo.

### Patrones de tupla (Tuple Patterns)

Algunos algoritmos dependen de varias entradas. Los **patrones de tupla** permiten hacer cambios en función de varios valores, expresados como una [tupla](../tuples.md).  El código siguiente muestra una expresión switch del juego *piedra, papel, tijeras*:

```csharp
public static string RockPaperScissors(string first, string second)
    => (first, second) switch
    {
        ("rock", "paper") => "rock is covered by paper. Paper wins.",
        ("rock", "scissors") => "rock breaks scissors. Rock wins.",
        ("paper", "rock") => "paper covers rock. Paper wins.",
        ("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
        ("scissors", "rock") => "scissors is broken by rock. Rock wins.",
        ("scissors", "paper") => "scissors cuts paper. Scissors wins.",
        (_, _) => "tie"
    };
```

Los mensajes indican el ganador. El caso de descarte representa las tres combinaciones de valores equivalentes, u otras entradas de texto.

### Patrones posicionales (Positional Patterns)

Algunos tipos incluyen un método `Deconstruct` que deconstruye sus propiedades en variables discretas. Cuando un método `Deconstruct` es accesible, puede usar **patrones posicionales** para inspeccionar las propiedades del objeto y usar esas propiedades en un patrón.  Tenga en cuenta la siguiente clase `Point` que incluye un método `Deconstruct` con el objetivo de crear variables discretas para `X` y `Y`:

```csharp
public class Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y) => (X, Y) = (x, y);

    public void Deconstruct(out int x, out int y) =>
        (x, y) = (X, Y);
}
```

Además, tenga en cuenta la siguiente enumeración, que representa diversas posiciones de un cuadrante:

```csharp
public enum Quadrant
{
    Unknown,
    Origin,
    One,
    Two,
    Three,
    Four,
    OnBorder
}
```

El método siguiente usa el **patrón posicional** para extraer los valores de `x` y `y`. A continuación, usa una cláusula `when` para determinar el valor `Quadrant` del punto:

```csharp
static Quadrant GetQuadrant(Point point) => point switch
{
    (0, 0) => Quadrant.Origin,
    var (x, y) when x > 0 && y > 0 => Quadrant.One,
    var (x, y) when x < 0 && y > 0 => Quadrant.Two,
    var (x, y) when x < 0 && y < 0 => Quadrant.Three,
    var (x, y) when x > 0 && y < 0 => Quadrant.Four,
    var (_, _) => Quadrant.OnBorder,
    _ => Quadrant.Unknown
};
```

El patrón de descarte del modificador anterior coincide cuando `x` o `y` es 0, pero no ambos. Una expresión switch debe generar un valor o producir una excepción. Si ninguno de los casos coincide, la expresión switch produce una excepción. El compilador genera una advertencia si no cubre todos los posibles casos en la expresión switch.

Puede explorar las técnicas de coincidencia de patrones en este [tutorial avanzado sobre la coincidencia de patrones](../tutorials/pattern-matching.md).

## Declaraciones using (Using statements)

Una **declaración using** es una declaración de variable precedida por la palabra clave `using`. Indica al compilador que la variable que se declara debe eliminarse al final del ámbito de inclusión. Por ejemplo, considere el siguiente código que escribe un archivo de texto:

```csharp
static int WriteLinesToFile(IEnumerable<string> lines)
{
    using var file = new System.IO.StreamWriter("WriteLines2.txt");
    // Notice how we declare skippedLines after the using statement.
    int skippedLines = 0;
    foreach (string line in lines)
    {
        if (!line.Contains("Second"))
        {
            file.WriteLine(line);
        }
        else
        {
            skippedLines++;
        }
    }
    // Notice how skippedLines is in scope here.
    return skippedLines;
    // file is disposed here
}
```

En el ejemplo anterior, el archivo se elimina cuando se alcanza la llave de cierre del método. Ese es el final del ámbito en el que se declara `file`. El código anterior es equivalente al siguiente código que usa las [instrucciones using](../language-reference/keywords/using-statement.md) clásicas:

```csharp
static int WriteLinesToFile(IEnumerable<string> lines)
{
    // We must declare the variable outside of the using block
    // so that it is in scope to be returned.
    int skippedLines = 0;
    using (var file = new System.IO.StreamWriter("WriteLines2.txt"))
    {
        foreach (string line in lines)
        {
            if (!line.Contains("Second"))
            {
                file.WriteLine(line);
            }
            else
            {
                skippedLines++;
            }
        }
    } // file is disposed here
    return skippedLines;
}
```

En el ejemplo anterior, el archivo se elimina cuando se alcanza la llave de cierre asociada con la instrucción `using`.

En ambos casos, el compilador genera la llamada a `Dispose()`. El compilador genera un error si la expresión de la instrucción `using` no se puede eliminar.

## Funciones locales estáticas (Static local functions)

Ahora puede agregar el modificador `static` a funciones locales para asegurarse de que la función local no captura (hace referencia a) las variables del ámbito de inclusión. Si lo hace, se generará un error `CS8421` en el que se indicará que una función local estática no puede contener una referencia a \<variable>.

Observe el código siguiente. La función local `LocalFunction` accede a la variable `y`, declarada en el ámbito de inclusión (el método `M`). Por lo tanto, `LocalFunction` no se puede declarar con el modificador `static`:

```csharp
int M()
{
    int y;
    LocalFunction();
    return y;

    void LocalFunction() => y = 0;
}
```

El código siguiente contiene una función local estática. Puede ser estática porque no accede a las variables del ámbito de inclusión:

```csharp
int M()
{
    int y = 5;
    int x = 7;
    return Add(x, y);

    static int Add(int left, int right) => left + right;
}
```

## Structs ref descartables (Disposable ref structs)

Un elemento `struct` declarado con el modificador `ref` no puede implementar ninguna interfaz y, por tanto, no puede implementar <xref:System.IDisposable>. Por lo tanto, para permitir que se elimine un elemento `ref struct`, debe tener un método `void Dispose()` accesible. Esta característica también se aplica a las declaraciones `readonly ref struct`.

## Tipos de referencia que aceptan valores NULL (Nullable reference types)

Dentro de un contexto de anotación que acepta valores NULL, cualquier variable de un tipo de referencia se considera un **tipo de referencia que no acepta valores NULL**. Si quiere indicar que una variable puede ser NULL, debe anexar `?` al nombre del tipo para declarar la variable como un **tipo de referencia que acepta valores NULL**.

En los tipos de referencia que no aceptan valores NULL, el compilador usa el análisis de flujo para garantizar que las variables locales se inicializan en un valor no NULL cuando se declaran. Los campos se deben inicializar durante la construcción. Si la variable no se establece mediante una llamada a alguno de los constructores disponibles o por medio de un inicializador, el compilador genera una advertencia. Además, los tipos de referencia que no aceptan valores NULL no pueden tener asignado un valor que podría ser NULL.

Los tipos de referencia que aceptan valores NULL no se comprueban para garantizar que se asignan o inicializan como NULL. Sin embargo, el compilador usa el análisis de flujo para garantizar que cualquier variable de un tipo de referencia que acepta valores NULL se compara con NULL antes de acceder a ella o de asignarse a un tipo de referencia que no los acepta.

Puede aprender más sobre la característica en la introducción a los [tipos de referencia que aceptan valores NULL](../nullable-references.md). Pruébela por su cuenta en una nueva aplicación en este [tutorial de tipos de referencia que aceptan valores NULL](../tutorials/nullable-reference-types.md). Puede encontrar más información sobre los pasos para migrar una base de código existente para hacer uso de los tipos de referencia que aceptan valores NULL en el [tutorial sobre la migración de una aplicación para usar tipos de referencia que aceptan valores NULL](../tutorials/upgrade-to-nullable-references.md).

## Secuencias asincrónicas (Asynchronous Streams)

A partir de C# 8.0, puede crear y consumir secuencias de forma asincrónica. Un método que devuelve una secuencia asincrónica tiene tres propiedades:

1. Se declara con el modificador `async`.
1. Devuelve <xref:System.Collections.Generic.IAsyncEnumerable%601>.
1. El método contiene instrucciones `yield return` que devuelven elementos sucesivos de la secuencia asincrónica.

Para consumir una secuencia asincrónica es necesario agregar la palabra clave `await` delante de la palabra clave `foreach` al enumerar los elementos de la secuencia. Para agregar la palabra clave `await` es necesario declarar el método que enumera la secuencia asincrónica con el modificador `async` y devolver un tipo permitido para un método `async`. Normalmente, esto significa devolver <xref:System.Threading.Tasks.Task> o <xref:System.Threading.Tasks.Task%601>. También puede ser <xref:System.Threading.Tasks.ValueTask> o <xref:System.Threading.Tasks.ValueTask%601>. Un método puede consumir y producir una secuencia asincrónica, lo que significa que devolvería <xref:System.Collections.Generic.IAsyncEnumerable%601>. El siguiente código genera una secuencia de 0 a 19, con una espera de 100 ms entre la generación de cada número:

```csharp
public static async System.Collections.Generic.IAsyncEnumerable<int> GenerateSequence()
{
    for (int i = 0; i < 20; i++)
    {
        await Task.Delay(100);
        yield return i;
    }
}
```

Se enumeraría la secuencia mediante la instrucción `await foreach`:

```csharp
await foreach (var number in GenerateSequence())
{
    Console.WriteLine(number);
}
```

Puede probar secuencias asincrónicas por su cuenta en nuestro tutorial sobre la [creación y consumo de secuencias asincrónicas](../tutorials/generate-consume-asynchronous-stream.md). Los elementos de secuencia se procesan de forma predeterminada en el contexto capturado. Si quiere deshabilitar la captura del contexto, use el método de extensión <xref:System.Threading.Tasks.TaskAsyncEnumerableExtensions.ConfigureAwait%2A?displayProperty=nameWithType>. Para obtener más información sobre los contextos de sincronización y la captura del contexto actual, vea el artículo sobre el [consumo del patrón asincrónico basado en tareas](../../standard/asynchronous-programming-patterns/consuming-the-task-based-asynchronous-pattern.md).

## Índices y rangos (Indexes and ranges)

Los índices y rangos proporcionan una sintaxis concisa para acceder a elementos únicos o intervalos en una secuencia.

Esta compatibilidad con lenguajes se basa en dos nuevos tipos y dos nuevos operadores:

- <xref:System.Index?displayProperty=nameWithType> representa un índice en una secuencia.
- El índice desde el operador final `^`, que especifica que un índice es relativo al final de la secuencia.
- <xref:System.Range?displayProperty=nameWithType> representa un subrango de una secuencia.
- El operador de intervalo `..`, que especifica el inicio y el final de un intervalo como sus operandos.

Comencemos con las reglas de índices. Considere un elemento `sequence` de matriz. El índice `0` es igual que `sequence[0]`. El índice `^0` es igual que `sequence[sequence.Length]`. Tenga en cuenta que `sequence[^0]` produce una excepción, al igual que `sequence[sequence.Length]`. Para cualquier número `n`, el índice `^n` es igual que `sequence.Length - n`.

Un rango especifica el *inicio* y el *final* de un intervalo. El inicio del rango es inclusivo, pero su final es exclusivo, lo que significa que el *inicio* se incluye en el rango, pero el *final* no. El rango `[0..^0]` representa todo el intervalo, al igual que `[0..sequence.Length]` representa todo el intervalo.

Veamos algunos ejemplos. Tenga en cuenta la siguiente matriz, anotada con su índice desde el principio y desde el final:

```csharp
var words = new string[]
{
                // index from start    index from end
    "The",      // 0                   ^9
    "quick",    // 1                   ^8
    "brown",    // 2                   ^7
    "fox",      // 3                   ^6
    "jumped",   // 4                   ^5
    "over",     // 5                   ^4
    "the",      // 6                   ^3
    "lazy",     // 7                   ^2
    "dog"       // 8                   ^1
};              // 9 (or words.Length) ^0
```

Puede recuperar la última palabra con el índice `^1`:

```csharp
Console.WriteLine($"The last word is {words[^1]}");
// writes "dog"
```

El siguiente código crea un subrango con las palabras "quick", "brown" y "fox". Va de `words[1]` a `words[3]`. El elemento `words[4]` no se encuentra en el intervalo.

```csharp
var quickBrownFox = words[1..4];
```

El siguiente código crea un subrango con "lazy" y "dog". Incluye `words[^2]` y `words[^1]`. El índice del final `words[^0]` no se incluye:

```csharp
var lazyDog = words[^2..^0];
```

En los ejemplos siguientes se crean rangos con final abierto para el inicio, el final o ambos:

```csharp
var allWords = words[..]; // contains "The" through "dog".
var firstPhrase = words[..4]; // contains "The" through "fox"
var lastPhrase = words[6..]; // contains "the", "lazy" and "dog"
```

También puede declarar rangos como variables:

```csharp
Range phrase = 1..4;
```

El rango se puede usar luego dentro de los caracteres `[` y `]`:

```csharp
var text = words[phrase];
```

No solo las matrices admiten índices y rangos. También puede usar índices y rangos con [string](../language-reference/builtin-types/reference-types.md#the-string-type), <xref:System.Span%601> o <xref:System.ReadOnlySpan%601>. Para más información, consulte [Compatibilidad con tipos para los índices y los rangos](../tutorials/ranges-indexes.md#type-support-for-indices-and-ranges).

Puede explorar más información acerca de los índices y los intervalos en el tutorial sobre [índices e intervalos](../tutorials/ranges-indexes.md).

## Asignación de uso combinado de NULL (Null-coalescing assignment)

C# 8.0 presenta el operador de asignación de uso combinado de `??=`. Puede usar el operador `??=` para asignar el valor de su operando derecho al operando izquierdo solo si el operando izquierdo se evalúa como `null`.

```csharp
List<int> numbers = null;
int? i = null;

numbers ??= new List<int>();
numbers.Add(i ??= 17);
numbers.Add(i ??= 20);

Console.WriteLine(string.Join(" ", numbers));  // output: 17 17
Console.WriteLine(i);  // output: 17
```

Para obtener más información, consulte [Operadores ?? y ??](../language-reference/operators/null-coalescing-operator.md).

## Tipos construidos no administrados (Unmanaged constructed types)

En C# 7.3 y versiones anteriores, un tipo construido (es decir, un tipo que incluye al menos un argumento de tipo) no puede ser un [tipo no administrado](../language-reference/builtin-types/unmanaged-types.md). A partir de C# 8.0, un tipo de valor construido no está administrado si solo contiene campos de tipos no administrados.

Por ejemplo, dada la siguiente definición del tipo genérico `Coords<T>`:

```csharp
public struct Coords<T>
{
    public T X;
    public T Y;
}
```

el tipo `Coords<int>` es un tipo no administrado en C# 8.0 y versiones posteriores. Al igual que en el caso de cualquier tipo no administrado, puede crear un puntero a una variable de este tipo o [asignar un bloque de memoria en la pila](../language-reference/operators/stackalloc.md) para las instancias de este tipo:

```csharp
Span<Coords<int>> coordinates = stackalloc[]
{
    new Coords<int> { X = 0, Y = 0 },
    new Coords<int> { X = 0, Y = 3 },
    new Coords<int> { X = 4, Y = 0 }
};
```

Para más información, consulte [Tipos no administrados](../language-reference/builtin-types/unmanaged-types.md).

## Stackalloc en expresiones anidadas

A partir de C# 8.0, si el resultado de una expresión [stackalloc](../language-reference/operators/stackalloc.md) es del tipo <xref:System.Span%601?displayProperty=nameWithType> o <xref:System.ReadOnlySpan%601?displayProperty=nameWithType>, puede usar la expresión `stackalloc` en otras expresiones:

```csharp
Span<int> numbers = stackalloc[] { 1, 2, 3, 4, 5, 6 };
var ind = numbers.IndexOfAny(stackalloc[] { 2, 4, 6 ,8 });
Console.WriteLine(ind);  // output: 1
```

## Mejora de las cadenas textuales interpoladas (Interpolated verbatim strings)

El orden de los tokens `$` y `@` en las cadenas textuales [interpoladas](../language-reference/tokens/interpolated.md) puede ser cualquiera: tanto `$@"..."` como `@$"..."` son cadenas textuales interpoladas válidas. En versiones de C# anteriores, el token `$` debe aparecer delante del token `@`.