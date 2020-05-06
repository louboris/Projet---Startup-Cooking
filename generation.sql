-- -------------- Script de création de la base de donnée pour le projet d'année 3 en base de donnée. LOUCHART BORIS 

create database projetcook;

use projetcook;

-- Create TABLE for cooking
create table client(username VARCHAR(20) unique, password char(40) character set ascii not null,nom VARCHAR(30), numero INT primary key, createur int default 0, cookpoint int default 0);
create table recette(recetteID int auto_increment not null primary key, nom varchar(50),type varchar(20),descriptif varchar(256),prix int , remuneration int default 2,createur int, foreign key(createur) references client(numero), palier int default 0);
create table commande(commandeID INT auto_increment primary key, numeroClient int, recetteID int, quantite int, foreign key(numeroClient) references client(numero),foreign key(recetteID) references recette(recetteID),date datetime);
create table ingredient(ingredientID int auto_increment not null primary key, nom varchar(50), categorie varchar(20), unite varchar(20), stockActuel int, stockMin int, stockMax int, fournisseur varchar(50), refFournisseur varchar(50),lastUse date,majdate date);
create table ingredient_recette(irID int auto_increment not null primary key, recetteID int, ingredientID int,quantite int, foreign key (recetteID) references recette(recetteID),foreign key(ingredientID) references ingredient(ingredientID) );

-- INSERT 
INSERT into client values('a',SHA1('a'),'Boris', 0752548215,1,1);
INSERT into client values('b',SHA1('b'),'Alexandre', 0767171485,0,1);
INSERT into client values('admin',SHA1('admin'),'admin', 0767171486,1,1);

INSERT INTO ingredient(nom,categorie,unite,stockActuel,stockMin,stockMax,fournisseur,refFournisseur,lastUse,majdate) values (Pomme de terre,legume,u,10,5,15,fourniseeur de Pomme de terre,pdt.com,date(now()),date(now())) ;
INSERT INTO ingredient(nom,categorie,unite,stockActuel,stockMin,stockMax,fournisseur,refFournisseur,lastUse,majdate) values (Tomate,legume,u,10,5,15,fournisseur de tomate,tomate.com,date(now()),date(now())) ;

insert into recette(nom,type,descriptif,prix,createur) values (Salade de PdT,salade,Salade de pomme de terre avec des tomates a la sauce cesar,30,752548215);

insert into ingredient_recette(recetteID,ingredientID,quantite) values (1,1,5);
insert into ingredient_recette(recetteID,ingredientID,quantite) values (1,2,3);

-- STORED PROCEDURE

DELIMITER 
 -- Permet de passer la commande, prend en parametre ID de la commande et la personne qui commande et effectue les actions necessaires a la commande
CREATE PROCEDURE PasserCommande(
    IN numero int, recetteC int, quantiteC int, prix int
)
BEGIN
insert into commande(numeroClient,recetteID,quantite,date) values(numero,recetteC,quantiteC,now());
-- Remuneration
update client join recette on client.numero = recette.createur set client.cookpoint = recette.remuneration  quantiteC where recette.recetteID = recetteC;

-- Premier palier de commande (augmente de 2 cook si total commande 10)
update recette set recette.prix = recette.prix + 2, palier = 1 where recette.recetteID = recetteC and 10 = (select sum(commande.quantite) from commande where commande.recetteID = recetteC) and recette.palier = 0;

-- Deuxième palier augmente de 5 cook si nb commande dépasse 50 + rem cdr 4 cook
update recette set recette.prix = recette.prix + 5 , recette. remuneration = 4 , recette.palier = 2 where recette.recetteID = recetteC and 50 = (select sum(commande.quantite) from commande where commande.recetteID = recetteC) and recette.palier = 1;

-- Retirer des stocks le nombre correspondrant a la recette commandée
update ingredient_recette join ingredient on ingredient_recette.ingredientID = ingredient.ingredientID set ingredient.stockActuel = ingredient.stockActuel - (ingredient_recette.quantite  quantiteC) where recetteID = recetteC;

END 
DELIMITER ;


DELIMITER 
 -- Permet de supprimer une recette avec les commandes associées (problème de foreign key)
 CREATE PROCEDURE SupprimerRecette(
    IN deleteID int
)
BEGIN
DELETE FROM ingredient_recette where ingredient_recette.recetteID = deleteID;
DELETE FROM commande where deleteID = commande.recetteID;
DELETE FROM recette where deleteID = recette.recetteID;
END  
DELIMITER ;


DELIMITER 
-- Permet de supprimer un CDR avec les recettes qu'il a crée
 CREATE PROCEDURE SupprimerCdr(
    IN deleteID int
)
BEGIN
SET SQL_SAFE_UPDATES=0;

DELETE FROM ingredient_recette USING ingredient_recette JOIN recette WHERE ingredient_recette.recetteID = recette.recetteID AND recette.createur = 767171485;


DELETE FROM commande using commande join recette on (commande.recetteID = recette.recetteID) where deleteID = recette.createur;
DELETE FROM recette where deleteID = recette.createur;
SET SQL_SAFE_UPDATES=1;

delete from client where numero = deleteID;

END 
 
DELIMITER ;

DELIMITER 
-- Permet de DownGrader un CDR et de supprimer les recettes qu'il a crée
 CREATE PROCEDURE DownGradeCdr(
    IN deleteID int
)
BEGIN
SET SQL_SAFE_UPDATES=0;

DELETE FROM ingredient_recette USING ingredient_recette JOIN recette WHERE ingredient_recette.recetteID = recette.recetteID AND recette.createur = 767171485;


DELETE FROM commande using commande join recette on (commande.recetteID = recette.recetteID) where deleteID = recette.createur;
DELETE FROM recette where deleteID = recette.createur;
SET SQL_SAFE_UPDATES=1;
update client set createur = 0 where numero = deleteID;


END 
 
DELIMITER ;



DELIMITER 
 -- Permet de supprimer une recette avec les commandes associées (problème de foreign key)
 CREATE PROCEDURE Ajouter_Ingredient_Recette(
    IN vrecetteID int, vingredientID int,vquantite int
)
BEGIN
insert into ingredient_recette(recetteID,ingredientID,quantite) values (vrecetteID,vingredientID,vquantite);
update ingredient set ingredient.stockMin = (ingredient.stockMin  2) + 3  vquantite, stockMax = stockMax + (2 vquantite) where ingredient.ingredientID = vingredientID;

END  
DELIMITER ;



DELIMITER 
-- Permet de faire la Réa de la semaine et editer la commande de la semaine grace a View crée plus bas dans la catégorie view
 CREATE PROCEDURE ReaHebdo()
BEGIN

update ingredient set stockMin = (stockMin  2), stockMax = (stockMax  2 )  where datediff(now(),lastUse)  30;
update ingredient set majdate = now() where majdate  now();

END 
 
DELIMITER ;


--                VIEWS

create view bestWeekCreator as
select client.username, client.nom, client.numero , t2.qt from client join (select recette.createur, sum(res.qt) as qt from recette join (select commande.recetteID, sum(commande.quantite) as qt from commande where week(now()) = week(commande.date) group by commande.recetteID) as res on res.recetteID = recette.recetteID group by recette.createur order by qt desc limit 1) as t2 on t2.createur = client.numero ;



create view cdrOr as
select username from client join (select recette.createur, sum(res.qt) as qt from recette join (select commande.recetteID, sum(commande.quantite) as qt from commande group by commande.recetteID) as res on res.recetteID = recette.recetteID group by recette.createur order by qt desc limit 1) as res on res.createur = client.numero;


create view ListeCommande as
select fournisseur, refFournisseur, nom, ingredientID, (stockMax - stockActuel) as qtCommandée from ingredient where ingredient.stockActuel  stockMin order by ingredient.fournisseur;
-- 
-- select

select  from ingredient;
select  from client;
select  from recette;
select  from commande;
select  from recette;
select  from ingredient_recette;