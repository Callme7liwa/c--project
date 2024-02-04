# Gestionnaire de Contacts .NET

## Introduction

Ce projet propose une application de gestion de contacts en utilisant la plateforme .NET. Il offre des fonctionnalités avancées telles que la gestion hiérarchique de dossiers, la sérialisation en XML et en binaire, ainsi que le cryptage des données.

## Structure du Projet

Le projet est organisé en trois composants distincts :

1. **gestion_contact** : Contient l'application console pour interagir avec le gestionnaire de contacts.
   
2. **gestion_contact_data** : Comprend la structure de données du gestionnaire de contacts, avec les classes `Dossier` et `Contact`.

3. **gestion_contact_serialisation** : Contient les classes liées à la sérialisation des données, permettant de sauvegarder et charger l'arbre de fichiers en XML ou en binaire, avec la possibilité de crypter les données.

## Structures de Données

Les principales structures de données sont définies dans le projet `gestion_contact_data` :

- **Fichier** : Possède une date de création, une date de modification et un nom.
- **Dossier** : Hérite de la classe `Fichier` et contient un dossier parent, une liste de fichiers (contacts ou dossiers), et offre la possibilité de remonter dans l'arbre de fichiers.
- **Contact** : Hérite de la classe `Fichier` et possède un prénom, un mail, une société et un lien.

## Sérialisation

La sérialisation est réalisée en XML ou en binaire, avec la possibilité de chiffrer les données. L'utilisateur peut saisir un mot de passe comme clé de chiffrement, qui sera utilisé lors de la sérialisation et demandé lors de la déserialisation. Les fichiers chiffrés sont identifiés par '_encrypted' dans leur nom.

## Localisation des Fichiers Sérialisés

Le programme crée un dossier dans le répertoire Mes Documents de l'utilisateur pour stocker et récupérer tous les fichiers sérialisés.

## Gestion des Dépendances Circulaires

Pour éviter des dépendances circulaires lors de la sérialisation XML, l'attribut `Parent` des dossiers est ignoré. Le chaînage est refait manuellement après la récupération de l'arbre de fichiers.

## Conclusion

Le projet "Gestionnaire de contacts" offre une solution complète pour la gestion avancée des contacts avec des fonctionnalités de sérialisation, de cryptage et une structure de données bien définie. L'application console fournit une interface utilisateur simple et efficace pour interagir avec le gestionnaire de contacts.
