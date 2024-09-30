Le projet est en version net7.0

Pour lancer le projet :
- Vérifier la présence des package suivants pour la version 7.0.11 avec la commande "dotnet list package" : 
    * Microsoft.EntityFrameworkCore
    * Microsoft.EntityFrameworkCore.Sqlite
    * Microsoft.EntityFrameworkCore.Tools
- Si ces packages ne sont pas présents, exécuter les lignes suivantes : 
    * dotnet add package Microsoft.EntityFrameworkCore --version 7.0.11
    * dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 7.0.11
    * dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.11

- Pour la migration et l'initialisation de la base de données liée à ce projet, exécuter les commandes suivantes : 
    * dotnet ef migrations add InitialCreate
    * dotnet ef database update

- Pour lancer le projet, exécuter la commande : dotnet run
- Dans un naviagteur, saisir l'url : http://localhost:5127/swagger/index.html

Utilisation du swagger :
- Dans l'onglet swagger, deux endpoints vous sont proposés : 
    * GetAllAds : permet la récupération des dernières offres pour la ville de Bordeaux depuis la base 
    de données si des offres y sont sockées sinon en faisant appel à l'Api JobiJoba. 
    * RefreshAds : permet le rafraichissement des offres présentes dans la base de données à partir de l'api JobiJoba

