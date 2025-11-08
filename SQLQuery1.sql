-- Create Database
create DATABASE Gestion_Pharmacie;
GO

USE Gestion_Pharmacie;
GO

-- Table: utilisateur
CREATE TABLE utilisateur (
    email NVARCHAR(100) primary key,
    nom NVARCHAR(100),
    prenom NVARCHAR(100),
    adrees text ,
    ville NVARCHAR(100),
    numero_telephone NVARCHAR(20) unique,
    mot_De_Passe text,
    role NVARCHAR(50),
    -- admin , client , livreur , fourniseur 
    date_Naissance DATE ,
    etat NVARCHAR(50) 
);


-- Table: Livreur
CREATE TABLE Livreur (
    livreur_email NVARCHAR(100) primary key,
    etat NVARCHAR(50),
    FOREIGN KEY (livreur_email) REFERENCES utilisateur(email) 
);



-- Table: fournisseur
CREATE TABLE fournisseur (
    fournisseur_email NVARCHAR(100) primary key ,
    nom_laboratoire NVARCHAR(100) ,
    etat NVARCHAR(50),
    FOREIGN KEY (fournisseur_email) REFERENCES utilisateur(email)
);



-- Table: categorie
CREATE TABLE categorie (
    id_categorie INT PRIMARY KEY,
    nom_categorie NVARCHAR(100),
    discription NVARCHAR(MAX)
);

-- Table: medicament

CREATE TABLE medicament (
    id_medicament INT PRIMARY KEY,
    nom_Medicament NVARCHAR(100),
    description TEXT ,
    prix_unitaire DECIMAL(10, 2),
    prix_achat DECIMAL(10, 2),
    dosage NVARCHAR(100),
    date_creation DATETIME,
    date_modification DATETIME,
    statut NVARCHAR(50),
    prescription_requise NVARCHAR(50),
    forme_pharmaceutique NVARCHAR(100),
    id_categorie INT,
    FOREIGN KEY (id_categorie) REFERENCES categorie(id_categorie)
);


-- Table: Pharmacie
CREATE TABLE Pharmacie (
    id_pahramcie INT PRIMARY KEY,
    nom_Pharmacie NVARCHAR(100),
    telephone_Pharmacie NVARCHAR(50),
    adresse text,
    Ville NVARCHAR(100),
    heure_ouverture TIME,
    heure_fermeture TIME,
    permanance NVARCHAR(50)
);

-- Table: commande
CREATE TABLE commande (
    client NVARCHAR(100),
    Livreur NVARCHAR(100),
    id_medicament int ,
    id_pahramcie int ,
    quantite INT,
    date_commande DATETIME,
    date_reception DATETIME,
    statut NVARCHAR(50),
    remarques text,
    FOREIGN KEY(client) REFERENCES utilisateur(email), 
    FOREIGN KEY(Livreur) REFERENCES Livreur(livreur_email), 
    FOREIGN KEY(id_pahramcie) REFERENCES Pharmacie(id_pahramcie), 
    FOREIGN KEY(id_medicament) REFERENCES medicament(id_medicament), 
    primary key (client,id_pahramcie,id_medicament,date_commande)
);

-- Table: contenir
CREATE TABLE contenir (
    id_medicament INT,
    id_pahramcie INT,
    date_peremption DATE,
    quantite_stock INT,
    reference_medicament NVARCHAR(100),
    seuil_alerte INT ,
    numero_lot NVARCHAR(50),
    FOREIGN KEY(id_pahramcie) REFERENCES Pharmacie(id_pahramcie), 
    FOREIGN KEY(id_medicament) REFERENCES medicament(id_medicament), 
    primary key (id_medicament , id_pahramcie , numero_lot)

);
GO


CREATE TABLE fournir (
    fournisseur NVARCHAR(100),
    Livreur NVARCHAR(100), 
    id_medicament int ,
    id_pahramcie int ,
    quantite INT,
    date_commande DATETIME,
    date_reception DATETIME,
    statut NVARCHAR(50),
    remarques text,
    numero_lot NVARCHAR(50),
    FOREIGN KEY(fournisseur) REFERENCES fournisseur(fournisseur_email), 
    FOREIGN KEY(Livreur) REFERENCES Livreur(livreur_email), 
    FOREIGN KEY(id_pahramcie) REFERENCES Pharmacie(id_pahramcie), 
    FOREIGN KEY(id_medicament) REFERENCES medicament(id_medicament), 
    primary key (fournisseur,id_pahramcie,id_medicament,date_commande)
  );