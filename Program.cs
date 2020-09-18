using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using CsvHelper;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleProjTemp
{
    class Program
    {

        // TODO: add use of polymorphism and aggregation
        // TODO: methods into a class?
        // TODO: need use of subclasses/ child and parent classes
        // TODO: save to text file after close
        // TODO: clean up code and case names

        // set the player and shop bank amount
        static public int shopBank = 1000;
        static public int playerBank = 1000;

        // variable for psuedo count of weapons and armor
        static public int weaponNumber = 0;
        static public int armorNumber = 0;


        static void Main(string[] args)
        {
            Weapon.WeaponStruct[] shopWeapInv;
            Armor.ArmorStruct[] shopArmorInv;

            // create an empty list to add armor and weapons from shop
            List<Weapon.WeaponStruct> myWeaps = new List<Weapon.WeaponStruct>();
            List<Armor.ArmorStruct> myArmors = new List<Armor.ArmorStruct>();

            // using csvhelper load .csv files
            using (var reader = new StreamReader("WeaponsSpread1.csv"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    IEnumerable<Weapon.WeaponStruct> weaps = csv.GetRecords<Weapon.WeaponStruct>();
                    shopWeapInv = weaps.ToArray<Weapon.WeaponStruct>();
                }
            }
            using (var reader = new StreamReader("armor.csv"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    IEnumerable<Armor.ArmorStruct> suits = csv.GetRecords<Armor.ArmorStruct>();
                    shopArmorInv = suits.ToArray<Armor.ArmorStruct>();
                }
            }

            //convert weapon and armor array to list
            List<Weapon.WeaponStruct> weaponList = shopWeapInv.ToList();
            List<Armor.ArmorStruct> armorList = shopArmorInv.ToList();


            bool gameRunning = true;
            bool gettingName = false;
            Player player = new Player();

            //store intro message
            Prompt($"Welcome to Fantasy Fanatics!\n" +
                $"I have a wide variety of items once belonging to video games, comics, and movies.\n" +
                $"Dont ask how I got them, just enjoy it while you can.\n" +
                $"Dont hesitate to ask for 'help' if you need any assistance!\n");
            Prompt($"First things first,");

            while (!gettingName)
            {
                // get player info and store as header to save file
                Prompt($"May I have your name, please? It is for my records...");
                player.playerName = Console.ReadLine().Trim();
                Prompt($"");
                Prompt($"Where are you from?");
                player.location = Console.ReadLine();
                Prompt($"");
                Prompt($"{player.playerName}, from {player.location}? Correct? ('y' or 'n')");
                if (Console.ReadLine() == "y")
                {
                    Prompt($"");
                    Prompt($"Welcome, {player.playerName}!");
                    Prompt($"You have {playerBank} units in your bank");
                    Prompt($"");
                    gettingName = true;
                }
                else
                {
                    Prompt($"Sorry, I must have misheard...");
                }

            }


            while (gameRunning)
            {
                Prompt($"What would you like to do, {player.playerName}?");
                string input = Console.ReadLine().Trim().ToLower();

                bool inputCommandDealtWith = false;

                // user input command cases
                switch (input)
                {
                    // shop and show weapons
                    case "show weap":
                    case "weapons":
                    case "weap":
                    case "show w":
                        ShowWeapons(weaponList);
                        inputCommandDealtWith = true;
                        break;

                    // shows player inventory
                    case "inventory":
                    case "my bag":
                    case "my inv":
                    case "bag":
                    case "inv":
                    case "i":
                        PlayerArmorInv(myArmors);
                        PlayerWeaponInv(myWeaps);
                        inputCommandDealtWith = true;
                        break;

                    // sell weapon or armor
                    case "sell":
                        Prompt($"Pick an item to sell (w1-w10 or a1-a10)");
                        input = Console.ReadLine().Trim().ToLower();
                        char sellInput = input[0];
                        switch (sellInput)
                        {

                            case 'w':
                                int tmpWNum;
                                input = input.Replace('w', ' ');
                                int.TryParse(input, out tmpWNum);
                                Prompt($"\nYou chose to sell weapon number {tmpWNum}");
                                try
                                {
                                    SellWeap(myWeaps, weaponList, tmpWNum);
                                }
                                catch (ArgumentOutOfRangeException)
                                {

                                    Prompt($"You entered an invalid weapon selection\n" +
                                        $"Please pick again and dont forget to check your inventory to see what numbers correspond to each weapon\n" +
                                        $"_______________");
                                }
                                break;

                            case 'a':
                                input = input.Replace('a', ' ');
                                int tmpANum;
                                int.TryParse(input, out tmpANum);
                                Prompt($"\nYou chose to sell armor number {tmpANum}");
                                try
                                {
                                    SellArmor(myArmors, armorList, tmpANum);
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    Prompt($"You entered an invalid armor selection\n" +
                                       $"Please pick again and dont forget to check your inventory to see what numbers correspond to each armor\n" +
                                       $"_______________");
                                }
                                break;

                        }
                        break;

                    // shop and show armor
                    case "show armor":
                    case "defense":
                    case "armor":
                    case "show a":
                        ShowArmor(armorList);
                        inputCommandDealtWith = true;
                        break;

                    //full shop inventory, no buying
                    case "shop":
                    case "shop inv":
                    case "show":
                    case "s":
                        ShowWeapons(weaponList);
                        ShowArmor(armorList);
                        inputCommandDealtWith = true;
                        break;

                    // help commands and instructions 
                    case "help":
                    case "commands":
                    case "inputs":
                    case "h":
                        Prompt($"Use the valid commands below to shop the store\nRemember to type 'help' anytime you get stuck.");
                        Prompt($"__________________\nThe current valid commands you can type in are:\n" +
                            $"- show armor, show a, armor, defense  - shows current shop armor inventory \n" +
                            $"- inventory, my bag, my inv, bag, inv, i - shows current player inventory \n" +
                            $"- weapons, show weap, show w, weap - shows current shop weapon inventory \n" +
                            $"- shop, shop inv, show, s - shows the full available inventory \n" +
                            $"- commands, inputs, help, h  - shows the help screen and game instructions \n" +
                            $"- esc, quit, leave, bye - closes the shop and window \n" +
                            $"- w1-w10 - use w and a corresponding number 1-10, to view that specific numbred weapon\n" +
                            $"- a1-a10 - use a and a corresponding number 1-10, to view that specific numbred weapon\n" +
                            $"- y - confirm selection \n" +
                            $"- n - decline selection");
                        break;

                    // quit and esc commands
                    case "esc":
                    case "quit":
                    case "leave":
                    case "bye":
                        Prompt($"\nThanks for shopping with us! You ended with {playerBank} units in your bank, " +
                            $"{myWeaps.Count} weapon(s), and {myArmors.Count} piece(s) of armor.\n" +
                            $"Please come again.");
                        gameRunning = false;
                        break;

                }
                // gets 'a' or 'w' for choosing an item to view
                if (!inputCommandDealtWith)
                {
                    char firstLetter = input[0];


                    switch (firstLetter)
                    {

                        case 'w':
                            int tmpWNum;
                            input = input.Replace('w', ' ');
                            int.TryParse(input, out tmpWNum);
                            Prompt($"\nYou chose to view weapon number {tmpWNum}");
                            try
                            {
                                ViewAndBuyWeapon(weaponList, myWeaps, tmpWNum);
                            }
                            catch (ArgumentOutOfRangeException)
                            {

                                Prompt($"You entered an invalid weapon selection\n" +
                                    $"Please pick again and dont forget to check the weapon list to see what numbers correspond to each weapon\n" +
                                    $"_______________");
                            }
                            break;

                        case 'a':
                            input = input.Replace('a', ' ');
                            int tmpANum;
                            int.TryParse(input, out tmpANum);
                            Prompt($"\nYou chose to view armor number {tmpANum}");
                            try
                            {
                                ViewAndBuyArmor(armorList, myArmors, tmpANum);
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                Prompt($"You entered an invalid armor selection\n" +
                                   $"Please pick again and dont forget to check the armor list to see what numbers correspond to each armor\n" +
                                   $"_______________");
                            }
                            break;

                    }





                }
            }
            Console.ReadKey();
        }

        // shows all weapon names and numbers
        static public void ShowWeapons(List<Weapon.WeaponStruct> weaponList)
        {

            Prompt($"\n");
            Prompt($"\n----WEAPONS----\n     vvvvv    \n");
            foreach (Weapon.WeaponStruct tmpWeap in weaponList)
            {

                weaponNumber++;
                Prompt($"-{tmpWeap.Name}- w{weaponNumber}\n_______________");

            }
            weaponNumber = 0;
            Prompt($"\n     ^^^^^    \n----WEAPONS----\n");

            if (playerBank <= 0)
            {
                playerBank = 0;
                Prompt($"Uh oh! You don't have any units left..\nPlease sell something or come back when you have some more units!");

            }
        }

        //shows all armor names and numbers
        static public void ShowArmor(List<Armor.ArmorStruct> armorList)
        {
            Prompt($"\n");
            Prompt($"\n----ARMOR----\n     vvv    \n");
            foreach (Armor.ArmorStruct tmpArmor in armorList)
            {
                armorNumber++;
                Prompt($"-{tmpArmor.Name}- a{armorNumber}\n_______________");

            }
            armorNumber = 0;
            Prompt($"\n     ^^^    \n----ARMOR----\n");

            if (playerBank <= 0)
            {
                playerBank = 0;
                Prompt($"Uh oh! You don't have any units left..\nPlease sell something or come back when you have some more units!");

            }
        }

        // method to view a speciic weapon and asks to purchase
        static public void ViewAndBuyWeapon(List<Weapon.WeaponStruct> weaponList, List<Weapon.WeaponStruct> myWeaps, int indexNum)
        {
            Weapon.WeaponStruct tmpWeap = weaponList[indexNum - 1];


            Prompt($"____________________\n- {tmpWeap.Name} -\n" +
                $"Type: {tmpWeap.Type}\n" +
                $"Description:\n{tmpWeap.Info}\n" +
                $"Damage: {tmpWeap.AttackPwr} pts\n" +
                $"Rarity: {tmpWeap.Rarity}\n" +
                $"Cost: {tmpWeap.Price} units\n");

            if (playerBank <= 0)
            {
                playerBank = 0;
                Prompt($"Uh oh! You don't have any units left..\nPlease sell something or come back when you have some more units!");
                return;
            }
            Prompt($"Would you like to buy this weapon? ('y' or 'n')");
            string input = Console.ReadLine();
            if (input == "n" || input == "N")
            {
                return;
            }
            if (input != "y")
            {
                Prompt($"Oops! Looks like you entered and invalid response. \nPlease select your desired weapon and try again\n" +
                    $"*Remember*: \nenter 'y' or 'Y' to buy item\nenter 'n' or 'N' to decline\n___________________");
                return;
            }
            if (input == "y" || input == "Y")
            {
                if (playerBank < tmpWeap.Price)
                {

                    Prompt($"Hey! You dont have any enough units left to purchase that!");
                    Prompt($"You have {playerBank} units and this item is {tmpWeap.Price}");
                    return;
                }
                else
                {
                    weaponList.Remove(tmpWeap);
                    myWeaps.Add(tmpWeap);

                    playerBank -= tmpWeap.Price;
                    shopBank += tmpWeap.Price;


                    Prompt($"You now own {tmpWeap.Name}!\n" +
                        $"You currently have {playerBank} units left in your bank\n________________");
                    return;
                }
            }





        }

        // method to view a speciic armor and asks to purchase
        static public void ViewAndBuyArmor(List<Armor.ArmorStruct> armorList, List<Armor.ArmorStruct> myArmors, int indexNum)
        {


            Armor.ArmorStruct tmpArmor = armorList[indexNum - 1];
            Prompt($"____________________\n- {tmpArmor.Name} -\n" +
                $"Type: {tmpArmor.Type}\n" +
                $"Description:\n{tmpArmor.Info}\n" +
                $"Defense: {tmpArmor.Defense} pts\n" +
                $"Rarity: {tmpArmor.Rarity}\n" +
                $"Cost: {tmpArmor.Price} units\n");

            if (playerBank <= 0)
            {
                playerBank = 0;
                Prompt($"Uh oh! You don't have any units left..\nPlease sell something or come back when you have some more units!");
                return;
            }

            Prompt($"Would you like to buy this for protection? ('y' or 'n')");
            string input = Console.ReadLine();
            if (input == "n" || input == "N")
            {
                return;
            }
            if (input != "y")
            {
                Prompt($"Oops! Looks like you entered and invalid response. \nPlease select your desired method of defense and try again\n" +
                       $"*Remember*: \nenter 'y' or 'Y' to buy item\nenter 'n' or 'N' to decline\n___________________\n");
                return;
            }
            if (input == "y" || input == "Y")
            {
                if (playerBank < tmpArmor.Price)
                {

                    Prompt($"Hey! You dont have any enough units left to purchase that!");
                    Prompt($"You have {playerBank} units and this item is {tmpArmor.Price}");
                    return;
                }
                else
                {
                    armorList.Remove(tmpArmor);
                    myArmors.Add(tmpArmor);

                    playerBank -= tmpArmor.Price;
                    shopBank += tmpArmor.Price;


                    Prompt($"You now own {tmpArmor.Name}!\n" +
                        $"You currently have {playerBank} units left in your bank\n___________________\n");
                    return;
                }
            }

        }

        // method to sell a weapon
        static public void SellWeap(List<Weapon.WeaponStruct> myWeapInv, List<Weapon.WeaponStruct> weaponList, int indexNum)
        {
            Weapon.WeaponStruct tmpWeap = weaponList[indexNum - 1];

            Prompt($"____________________\n- {tmpWeap.Name} -\n" +
                $"Type: {tmpWeap.Type}\n" +
                $"Description:\n{tmpWeap.Info}\n" +
                $"Damage: {tmpWeap.AttackPwr} pts\n" +
                $"Rarity: {tmpWeap.Rarity}\n" +
                $"Cost: {tmpWeap.Price} units\n");

            Prompt($"Would you like to sell this item? ('y' or 'n')");
            string input = Console.ReadLine();

            if (input == "n" || input == "N")
            {
                Prompt($"You declined to sell this weapon\n");
                return;
            }
            if (input != "y")
            {
                Prompt($"Oops! Looks like you entered and invalid response. \nPlease try again\n" +
                       $"*Remember*: \nenter 'y' or 'Y' to sell selected item\nenter 'n' or 'N' to decline\n___________________\n");
                return;
            }
            if (input == "y" || input == "Y")
            {
                if (shopBank < tmpWeap.Price)
                {
                    Prompt($"Yikes. I dont have enough units to buy that back.");
                    Prompt($"The item is {tmpWeap.Price} units and I have only have {shopBank} units");
                    Prompt($"Sorry! Maybe if you buy something from me, I will have enough units then");
                    return;
                }
                else
                {
                    myWeapInv.Remove(tmpWeap);
                    weaponList.Add(tmpWeap);

                    playerBank += tmpWeap.Price;
                    shopBank -= tmpWeap.Price;

                    Prompt($"You sold {tmpWeap.Name} for {tmpWeap.Price} units!\n" +
                        $"You now have {playerBank} units in your bank\n___________________\n");
                    Prompt($"The shop bank is now at {shopBank}");
                }
            }
        }

        // method to sell a specific armor
        static public void SellArmor(List<Armor.ArmorStruct> myArmorInv, List<Armor.ArmorStruct> armorList, int indexNum)
        {
            Armor.ArmorStruct tmpArmor = myArmorInv[indexNum - 1];

            Prompt($"____________________\n- {tmpArmor.Name} -\n" +
                $"Type: {tmpArmor.Type}\n" +
                $"Description:\n{tmpArmor.Info}\n" +
                $"Defense: {tmpArmor.Defense} pts\n" +
                $"Rarity: {tmpArmor.Rarity}\n" +
                $"Cost: {tmpArmor.Price} units\n");

            Prompt($"Would you like to sell this item? ('y' or 'n')");
            string input = Console.ReadLine().Trim().ToLower();
            if (input == "n" || input == "N")
            {
                Prompt($"You declined to sell this armor\n");
                return;
            }
            if (input != "y")
            {
                Prompt($"Oops! Looks like you entered and invalid response. \nPlease try again\n" +
                       $"*Remember*: \nenter 'y' or 'Y' to sell selected item\nenter 'n' or 'N' to decline\n___________________\n");
                return;
            }
            if (input == "y" || input == "Y")
            {
                if (shopBank < tmpArmor.Price)
                {

                    Prompt($"Yikes. I dont have enough units to buy that back.");
                    Prompt($"The item is {tmpArmor.Price} units and I have only have {shopBank} ");
                    return;
                }
                else
                {
                    myArmorInv.Remove(tmpArmor);
                    armorList.Add(tmpArmor);

                    playerBank += tmpArmor.Price;
                    shopBank -= tmpArmor.Price;

                    Prompt($"You sold {tmpArmor.Name} for {tmpArmor.Price} units!\n" +
                        $"You now have {playerBank} units in your bank\n___________________\n");
                    Prompt($"The shop bank is now at {shopBank}");
                    return;
                }
            }
        }

        // shows all information of weapons in player inv
        static public void PlayerWeaponInv(List<Weapon.WeaponStruct> myWeaps)
        {
            int numOfWeap = 0;
            Prompt($"\n----MY WEAPONS----\n     vvvvvvvv    \n");
            foreach (Weapon.WeaponStruct myInv in myWeaps)
            {
                numOfWeap++;
                Prompt($"____________________\n- {myInv.Name} - w{numOfWeap}\n" +
                        $"Type: {myInv.Type}\n" +
                        $"Description:\n{myInv.Info}\n" +
                        $"Damage: {myInv.AttackPwr} pts\n" +
                        $"Rarity: {myInv.Rarity}\n" +
                        $"Cost: {myInv.Price} units");
            }
            Prompt($"\n     ^^^^^^^^    \n----MY WEAPONS----\n");
        }

        // shows all information of armor in player inv
        static public void PlayerArmorInv(List<Armor.ArmorStruct> myArmors)
        {
            int numOfArmor = 0;
            Prompt($"\n----MY ARMOR----\n     vvvvvvvv    \n");
            foreach (Armor.ArmorStruct myInv in myArmors)
            {
                numOfArmor++;
                Prompt($"____________________\n- {myInv.Name} - a{numOfArmor}\n" +
                        $"Type: {myInv.Type}\n" +
                        $"Description:\n{myInv.Info}\n" +
                        $"Defense: {myInv.Defense} pts\n" +
                        $"Rarity: {myInv.Rarity}\n" +
                        $"Cost: {myInv.Price} units");
            }
            numOfArmor = 0;
            Prompt($"\n     ^^^^^^^^    \n----MY ARMOR----\n");
        }

        // simple method for writing prompts
        static public void Prompt(string prompt)
        {
            Console.WriteLine(prompt);
        }

    }


}
