# AplikacjeBazodanowe

Jest to projekt blogu internetowego, gdzie można dodawać publikacje, zostawiać komentarze i odpowiedzi na nich. Publikacje są dodawane do kategorii.

## Uruchomienie projektu

Projekt wymaga następujące narzędzia:

- Visual Studio 2022
- .NET 6.0
- zainstalowaną instancję **SQL Server Express 2019** (domyślne ustawienia)
- narzędzie `dotnet-ef` do dokonania migracji (jest instalowane globalnie):

    żeby zainstalować go skorzystaj z `dotnet CLI`:

    ```PowerShell
    dotnet tool install --global dotnet-ef
    ```

1. Upewnij się, że opcja połączenia do bazy danych ci pasuje ([appsettings.json](./AplikacjeBazodanowe/appsettings.json)):

    ```JSON
    {
      "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=AplikacjeBazodanowe;Trusted_Connection=true;MultipleActiveResultSets=true",
      ...
    }
    ```

2. Skorzystaj z narzędzia `dotnet-ef` w folderze z plikiem **.csproj**, żeby dokonać migracji (przygotować bazę danych do uruchomienia aplikacji):

    ```PowerShell
    cd AplikacjeBazodanowe
    dotnet ef database update
    ```

3. Otwórz projekt w Visual Studio 2022 i uruchom go.

4. Domyślne dane do logowania znajdują się w [appsettings.json](./AplikacjeBazodanowe/appsettings.json):

    ```JSON
    "DefaultAdmin": {
      "Username": "admin",
      "Email": "admin@site.com",
      "Password": "password",
      "Role": "Admin"
    }
    ```

Dmytro Martsynenko 36427
