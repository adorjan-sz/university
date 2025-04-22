module Beadandó where
import Data.List
showState a = show a
showMage a = show a
eqMage a b =  a == b
showUnit a = show a
--showOneVOne a = show a 


type Name = String
type Health = Integer
type Spell = (Integer -> Integer)

type EnemyArmy = Army
type Amount = Integer


data State a = Alive a | Dead deriving Eq
instance Show a => Show (State a) where
    show (Alive a) = show a 
    show (Dead) = "Dead"

data Entity  = HaskellElemental Health | Golem Health deriving (Show,Eq)

data Mage  =  Master Name Health Spell
instance  Eq (Mage ) where
    (==) (Master name1 health1 spell1) (Master name2 health2 spell2) = name1==name2 && health1 == health2
instance Show (Mage ) where
    show (Master name health spell) = help name health where
        help n h
            |h < 5 = "Wounded " ++ n
            |otherwise =  n

data Unit = E (State Entity)  | M (State Mage) deriving Eq
instance Show (Unit) where
    show (M (Alive (Master name health spell)) ) = show ((Master name health spell))
    show (M (Dead ) ) = "Dead"
    show (E (Alive (HaskellElemental  health )) ) = show ((HaskellElemental  health ))
    show (E (Alive (Golem  health )) ) = show ((Golem  health ))
    show (E (Dead ) ) = "Dead"



life::Integer->Unit->Unit
life n (M (Alive (Master name health spell)) ) 
    |health+n <= 0 = M (Dead ) 
    |otherwise = (M (Alive (Master name (health+n) spell)) )
life n (E (Alive (HaskellElemental  health )) )
    |health+n <= 0 = E (Dead ) 
    |otherwise = (E (Alive (HaskellElemental  (health+n) )) )
life n (E (Alive (Golem  health )) )
    |health+n <= 0 = E (Dead ) 
    |otherwise = (E (Alive (Golem  (health+n) )) )
life n (M (Dead)) = (M (Dead))
life n (E (Dead)) = (E (Dead))


useSpell :: Unit->Unit->Unit
useSpell (M (Alive (Master name1 health1 spell1)) )  (M (Alive (Master name2 health2 spell2)) )
    |spell1 health2 <= 0 = (M(Dead))
    |otherwise=(M (Alive (Master name2 (spell1 health2) spell2)) )
useSpell (M (Alive (Master name1 health1 spell1)) ) (E (Alive (HaskellElemental  health2 )) )
    |spell1 health2<= 0 = (E (Dead))
    |otherwise= (E (Alive (HaskellElemental  (spell1 health2) )) )
useSpell (M (Alive (Master name1 health1 spell1)) ) (E (Alive (Golem  health2 )) )
    |spell1 health2<= 0 = (E (Dead))
    |otherwise= (E (Alive (Golem  (spell1 health2) )) )
useSpell _ (M (Dead)) = (M (Dead))
useSpell _ (E (Dead)) = (E (Dead))




type Army = [Unit]

papi = let 
    tunderpor enemyHP
        | enemyHP < 8 = 0
        | even enemyHP = div (enemyHP * 3) 4
        | otherwise = enemyHP - 3
    in Master "Papi" 126 tunderpor
java = Master "Java" 100 (\x ->  x - (mod x 9))
traktor = Master "Traktor" 20 (\x -> div (x + 10) ((mod x 4) + 1))
jani = Master "Jani" 100 (\x -> x - div x 4)
skver = Master "Skver" 100 (\x -> div (x+4) 2)
potionMaster = 
  let plx x
        | x > 85  = x - plx (div x 2)
        | x == 60 = 31
        | x >= 51 = 1 + mod x 30
        | otherwise = x - 7 
  in Master "PotionMaster" 170 plx

formationFix :: Army -> Army
formationFix lista = (filter (\x -> (show x) /= "Dead") lista) ++ (filter (\x -> (show x) == "Dead") lista)

over :: Army -> Bool
over lista = all (\x->show x == "Dead") lista

fight :: EnemyArmy -> Army -> Army
fight [] army = army
fight army [] = [] 
fight (e:es) (a:as) = (func e a) : (map (\x->func2 e x) (fight es as))

func e a = case e of
    (E (Dead ) ) -> a
    (M (Dead )) -> a
    (E (Alive (HaskellElemental  health )) ) -> (life (-3) a)
    (E (Alive (Golem  health )) ) -> (life (-1) a)
    (M (Alive (Master name health spell)) ) -> (useSpell e a)
func2 e a = case e of
    (E (Dead ) ) -> a
    (M (Dead )) -> a
    (E (Alive (HaskellElemental  health )) ) -> a
    (E (Alive (Golem  health )) ) -> a
    (M (Alive (Master name health spell)) ) -> (useSpell e a)

                                                

sumHealth::Army->Integer
sumHealth [] = 0
sumHealth lista=seged lista 0 where
    seged::Army->Integer->Integer
    seged [] acc = acc
    seged (x:xs) acc = case x of
        (E(Dead))-> seged xs (acc)
        (M(Dead))-> seged xs (acc)
        (E (Alive (HaskellElemental  health )) ) -> seged xs (health+acc)
        (E (Alive (Golem  health )) ) ->seged xs (health+acc)
        (M (Alive (Master name health spell)) ) -> seged xs (health+acc)

healthnum::Unit->Integer
healthnum x = case x of
        (E(Dead))-> 0
        (M(Dead))-> 0
        (E (Alive (HaskellElemental  health )) ) -> health
        (E (Alive (Golem  health )) ) ->health
        (M (Alive (Master name health spell)) ) -> health


atleastFives::Army->Bool
atleastFives [] = False
atleastFives lista = seged lista True where
    seged::Army->Bool->Bool
    seged [] True = True
    seged lista@(x:xs) logic 
        |(healthnum x) < 5= False
        |otherwise = seged xs True



haskellBlast :: Army -> Army
haskellBlast [] = []
haskellBlast lista
    |(length lista) <= 5 = map (\a->life (-5) a) lista
    |otherwise = seged  [] lista [] False 0 where
        seged :: Army->Army->Army->Bool->Integer->Army
        seged _ [] done _ _ = done
        seged before after@(x:xs) done logic count
            |(atleastFives(take 5 after)==True ) = before ++ (map (\u->life (-5) u) (take 5 after)) ++ (drop 5 after)
            |(atleastFives(take 5 after)==False && logic==False) = seged (before ++ [x]) xs (before ++ (map (\u->life (-5) u) (take 5 after)) ++ (drop 5 after)) True (sumHealth(take 5 after))
            |(atleastFives(take 5 after)==False) && logic==True && (count< (sumHealth(take 5 after)) ) = seged (before ++ [x]) xs (before ++ (map (\u->life (-5) u) (take 5 after)) ++ (drop 5 after)) True (sumHealth(take 5 after))
            |otherwise= seged (before ++ [x]) xs done True count


multiHeal :: Health -> Army -> Army
multiHeal _ [] = []
multiHeal n army
    |all (\x->(show x)=="Dead") army = army
    |n<0=army
multiHeal h army = seged h army [] where
    seged:: Health -> Army -> Army-> Army
    seged 0 army acc= acc ++ army
    seged h [] acc = seged h acc []
    seged h lista@(a:as) acc
        |show a == "Dead"  = seged h as (acc++[a])
        |otherwise = seged (h-1) as (acc++[life 1 a])




battle :: Army -> EnemyArmy -> Maybe Army 
battle army enemy
    |(over army) && (over enemy) = Nothing
    |(over army) = Just enemy
    |(over enemy) = Just army
    |otherwise= battle (formationFix( multiHeal 20 (haskellBlast (fight enemy army))))  (formationFix(fight army enemy))

chain :: Integer -> (Army, EnemyArmy) -> (Army, EnemyArmy)
chain n (army,enemy) = seged n (army,enemy) ([],[]) where
    seged:: Integer -> (Army, EnemyArmy) ->(Army, EnemyArmy) -> (Army, EnemyArmy)
    seged n (army,enemy) (aAcc,eAcc) 
        |n<=0 = ((aAcc ++ army),(eAcc ++ enemy))
    seged _ ([],[]) (aAcc,eAcc) = (aAcc,eAcc)
    seged n ([],(e:es)) (aAcc,eAcc) = ((aAcc),(eAcc ++ (e:es)))
    seged n ((a:as),[]) (aAcc,eAcc) = ((aAcc ++ ((life n a):as)),(eAcc))
    seged n ((a:as),(e:es)) (aAcc,eAcc) 
        |((show a) == "Dead") && ((show e) == "Dead") = seged (n) (as,es) ((aAcc ++ [(a)]),(eAcc ++ [e]))
        |((show a) == "Dead") && ((show e) /= "Dead") = seged (n-1) (as,es) ((aAcc ++ [a]),(eAcc ++ [(life (-1*(n)) e)]))
        |((show a) /= "Dead") && ((show e) == "Dead") = seged (n-1) (as,es) ((aAcc ++ [(life n a)]),(eAcc ++ [e]))
        |otherwise = seged (n-2) (as,es) ((aAcc ++ [(life n a)]),(eAcc ++ [(life (-1*(n-1)) e)]))

{-
battleWithChain :: Army -> EnemyArmy -> Maybe Army 
battleWithChain army enemy
    |(over army) && (over enemy) = Nothing
    |(over army) = Just enemy
    |(over enemy) = Just army
    |otherwise= battleWithChain  (formationFix(fst (chain 5  (multiHeal 20 (haskellBlast (fight enemy army)),(fight army enemy))))) (formationFix(snd  (chain 5  (multiHeal 20 (haskellBlast (fight enemy army)),(fight army enemy)))))
-}