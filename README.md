##Gestionnaire de contacts : Rapport de projet .NET

#Introduction
Le projet "Gestionnaire de contacts" en .NET a été conçu pour offrir une solution de gestion avancée des contacts avec des fonctionnalités telles que la structure hiérarchique de dossiers, la sérialisation en XML et en binaire, ainsi que le cryptage des données.

#Structure du Projet
Le projet est organisé en trois projets distincts dans Visual Studio :

gestion_contact : Contient l'application console utilisée pour interagir avec le gestionnaire de contacts.

gestion_contact_data : Comprend la structure de données du gestionnaire de contacts, notamment les classes Dossier et Contact dérivées de la classe Fichier.

gestion_contact_serialisation : Contient les classes nécessaires à la sérialisation des données, offrant la possibilité de sauvegarder et charger l'arbre de fichiers en XML ou en binaire, avec la fonctionnalité de cryptage.

#Structures de Données
Les principales structures de données sont définies dans le projet gestion_contact_data :

Fichier : Possède une date de création, une date de modification et un nom.
Dossier : Hérite de la classe Fichier et contient un dossier parent, une liste de fichiers (contacts ou dossiers), et offre la possibilité de remonter dans l'arbre de fichiers.
Contact : Hérite de la classe Fichier et possède un prénom, un mail, une société et un lien.
Sérialisation
La sérialisation est réalisée en XML ou en binaire, avec la possibilité de chiffrer les données. L'utilisateur peut saisir un mot de passe comme clé de chiffrement, qui sera utilisé lors de la sérialisation et demandé lors de la déserialisation. Les fichiers chiffrés sont identifiés par '_encrypted' dans leur nom.

Localisation des Fichiers Sérialisés
Le programme crée un dossier dans le répertoire Mes Documents de l'utilisateur pour stocker et récupérer tous les fichiers sérialisés.

Gestion des Dépendances Circulaires
Pour éviter des dépendances circulaires lors de la sérialisation XML, l'attribut Parent des dossiers est ignoré. Le chaînage est refait manuellement après la récupération de l'arbre de fichiers.

Conclusion
Le projet "Gestionnaire de contacts" offre une solution complète pour la gestion avancée des contacts avec des fonctionnalités de sérialisation, de cryptage et une structure de données bien définie. L'application console fournit une interface utilisateur simple et efficace pour interagir avec le gestionnaire de contacts.
