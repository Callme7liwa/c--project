# Gestionnaire de contacts : TP .net

Application de gestionnaire de contacts. Contient trois projets Visual Studio :
- `gestion_contact` contient l'application console
- `gestion_contact_data` contient la structure de données
- `gestion_contact_serialisation` contient les classes utilisées pour la serialisation

L'application console s'utilise par des commandes à entrer dans la console. Plusieurs alternatives sont acceptées pour chaque option.

## Structures de données
Les structures de données utilisées sont stockées dans le projet `gestionnaire_contact_data`. Pour faciliter le stockage des enfants dans chaque dossier, les classes `Dossier` et `Contact` dérivent de la classe `Fichier`.
- Un `Fichier` possède une date de création, une date de modification et un nom.
- Un `Dossier` possède un dossier parent pour pouvoir remonter dans l'arbre de fichier, ce qui n'est pas nécessaire pour un contact. Il possède aussi une liste de fichiers, qu'ils soient contacts ou dossiers.
- Un `Contact` possède un prénom, un mail, une société et un lien. Le mail d'un contact est vérifié à la création et doit respecter le format '---@---.---".

## Sérialisation
Un arbre de fichiers peut être sérialisé dans un fichier XML ou en format binaire. L'extension donnée dans le nom du fichier détermine le type de fichier. Pour chiffrer les données sérialisées, l'utilisateur doit saisir un mot de passe qui servira de clé de chiffrement. Ce mot de passe lui est redemandé lors de la déserialisation. Un fichier chiffré est identifié par '_encrypted' avant l'extension. Le vecteur d'initialisation est généré aléatoirement et écrit au début du fichier chiffré. Le programme crée un dossier dans le dossier `Mes Documents` de l'utilisateur où il stocke et récupère tous les fichiers sérialisés.
L'attribut Parent des dossiers est ignoré par la sérialisation en XML pour éviter les dépendances circulaires qui empêchaient la sérialisation. C'est pour cela qu'il est nécessaire de refaire le chaînage à la main une fois l'arbre de fichiers récupéré.
