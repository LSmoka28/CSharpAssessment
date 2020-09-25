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
using System.CodeDom;

namespace CSharpAssessmentProject
{
    class Program
    {
        // set the player and shop bank amount
        static public int shopBank = 1000;
        static public int playerBank = 1000;

        // variable for numbering of weapons and armor
        static public int weaponNumber = 0;
        static public int armorNumber = 0;

        // instance of the Player class to hold Player ID
        static public Player player;

        // main program method
        static void Main()
        {
            // create array of weapon and armor class structs before csv load and list conversion
            Weapon.WeaponStruct[] shopWeapInv;
            Armor.ArmorStruct[] shopArmorInv;

            // using csvhelper load .csv files before list conversion
            // loads weapons into shop inv from specified .csv
            using (var reader = new StreamReader("WeaponsSpread1.csv"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    IEnumerable<Weapon.WeaponStruct> weaps = csv.GetRecords<Weapon.WeaponStruct>();
                    shopWeapInv = weaps.ToArray<Weapon.WeaponStruct>();
                }
            }

            // loads armor into shop inv from specified .csv
            using (var reader = new StreamReader("armor.csv"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    IEnumerable<Armor.ArmorStruct> suits = csv.GetRecords<Armor.ArmorStruct>();
                    shopArmorInv = suits.ToArray<Armor.ArmorStruct>();
                }
            }

            // conversion of loaded weapon and armor arrays to a list
            List<Weapon.WeaponStruct> weaponList = shopWeapInv.ToList();
            List<Armor.ArmorStruct> armorList = shopArmorInv.ToList();

            // create an empty list to add armor and weapons from shop to player inventory
            List<Weapon.WeaponStruct> myWeaps = new List<Weapon.WeaponStruct>();
            List<Armor.ArmorStruct> myArmors = new List<Armor.ArmorStruct>();

            #region Store Introduction and Player ID
            // store intro message
            Prompt($"Welcome to Fantasy Fanatics!\n" +
                $"I have a wide variety of items once belonging to video games, comics, and movies\n" +
                $"Don't ask how I got them, just enjoy it while you can\n" +
                $"Don't hesitate to ask for 'help' if you need any assistance!\n");
            Prompt($"First things first,");

            // get player name and location, and check to confirm with player
            bool gettingName = false;
            while (!gettingName)
            {
                // get player name and location
                Prompt($"May I have your name, please? It is for my records...");
                string name = Console.ReadLine().Trim();
                Prompt($"\nWhere are you from?");
                Prompt($"City?");
                string city = Console.ReadLine().Trim();
                Prompt($"\nState or Country?");
                string stateOrCountry = Console.ReadLine().Trim();
                Prompt($"");

                if (name == "" || city == "" || stateOrCountry == "")
                {
                    Prompt($"Name/City/State/Country cannot be blank\nPlease enter valid characters\n");
                }
                else
                {
                    // add info to player after reading player name and location from console
                     player = new Player(name, new Address(city, stateOrCountry));

                    // display PlayerID and ask to confirm
                    player.Display();
                    Prompt($"\nYour name will be used for your save files\nIs this correct? ('y' or 'n')");
                    if (Console.ReadLine().Trim().ToLower() == "y")
                    {                      
                        Prompt($"\nWelcome, {name}!\n");
                        Prompt($"You have {playerBank} units in your bank\n");

                        gettingName = true;
                    }
                    else
                    {
                        Prompt($"Sorry, I must have misheard...");
                    }
                }
            }
            #endregion

            #region Main Game Loop
            bool gameRunning = true;
            while (gameRunning)
            {
                Prompt($"What would you like to do?");
                string input = Console.ReadLine().Trim().ToLower();

                // must include 'true' inputCommandDealtWith on each case calling a Show or PlayerInv method
                bool inputCommandDealtWith = false;

                // user input command cases
                switch (input)
                {
                    // shows shop weapon inventory using ShowWeapons method                   
                    case "show weap":
                    case "weapons":
                    case "weap":
                    case "show w":
                        ShowWeapons(weaponList);
                        inputCommandDealtWith = true;
                        break;

                    // shows shop armor inventory using ShowArmor method
                    case "show armor":
                    case "armor":
                    case "arm":
                    case "show a":
                        ShowArmor(armorList);
                        inputCommandDealtWith = true;
                        break;

                    // shows both shop inventories
                    case "shop inv":
                    case "shop":
                    case "show":
                    case "look":
                        ShowWeapons(weaponList);
                        ShowArmor(armorList);
                        inputCommandDealtWith = true;
                        break;

                    // shows both player inventories 
                    case "inventory":
                    case "my bag":
                    case "bag":
                    case "my inv":
                    case "inv":
                    case "i":
                        PlayerWeaponInv(myWeaps);
                        PlayerArmorInv(myArmors);
                        Prompt($"Bank: {playerBank} units\n");
                        inputCommandDealtWith = true;
                        break;

                    // sell weapon or armor, type w1-w10 or a1-a10 after initial command                    
                    // includes check for player selection out of list range
                    case "sell":
                    case "trade":
                        Prompt($"Pick an item to sell (w1-w10 or a1-a10)");
                        input = Console.ReadLine().Trim().ToLower();
                        char sellInput = input[0];
                        switch (sellInput)
                        {
                            // selects weapon in player inventory based on number
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
                                        $"Please pick again and dont forget to check your inventory to see what number corresponds to each weapon\n" +
                                        $"_______________");
                                }
                                break;

                            // selects armor in player inv based on number
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
                                       $"Please pick again and dont forget to check your inventory to see what number corresponds to each armor\n" +
                                       $"_______________");
                                }
                                break;                       
                        }
                        break;



                    // help commands and instructions 
                    case "help":
                    case "commands":
                    case "inputs":
                    case "h":
                        Prompt($"\nUse the valid commands below to shop the store\n" +
                            $"Don't forget to press your Enter/Return key after each command\n" +
                            $"And remember to type 'help' if you get stuck");
                        Prompt($"__________________\nThe current valid commands you can type in are: \n" +
                            $"- y - confirm a selection \n" +
                            $"- n - decline a selection \n" +
                            $"- w1-w10 - use 'w' and a corresponding number '1-10', to view a weapon in that slot\n" +
                            $"- a1-a10 - use 'a' and a corresponding number '1-10', to view an armor in that slot\n" +
                            $"- load - loads a .csv file for shop inv and a user inv by input of name used for save\n" +
                            $"- show armor, show a, armor, arm  - shows current shop armor inventory and prices \n" +
                            $"- weapons, show weap, show w, weap - shows current shop weapon inventory and prices \n" +
                            $"- inventory, my bag, bag, my inv, inv, i - shows current player inventory \n" +
                            $"- shop inv, shop, show, look - shows the full available shop inventory and prices\n" +
                            $"- sell, trade - sell items back to the shop. for a weapon: type (w1-w10) or an armor: type (a1-a10) \n" +
                            $"- commands, inputs, help, h  - shows this help screen and game instructions \n" +
                            $"- esc, quit, leave, save - asks to save files or quit without saving\n");
                        break;

                    // quit or save progress
                    case "esc":
                    case "quit":
                    case "leave":
                    case "save":
                        Prompt($"\nThank you for shopping with us! You ended with " +
                            $"{myWeaps.Count} weapon(s), and {myArmors.Count} piece(s) of armor\n" +
                            $"\nWould you like to save your progress?");
                        Prompt($"Enter 'y' if you would like to save\nEnter 'n' if you would like to quit without saving");
                        string saveOrQuit = Console.ReadLine().ToLower().Trim();
                        if (saveOrQuit == "y")
                        {
                            // if yes, saves the files with players name serving as the heading
                            // player weapon save
                            using (var writer = new StreamWriter($"{player.name}weap.csv"))
                            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                            {
                                csv.WriteRecords(myWeaps);
                            }
                            Prompt($"...");
                            Prompt($"Player weapon inventory file saved as {player.name}weap.csv\n");

                            // player armor save
                            using (var writer = new StreamWriter($"{player.name}armor.csv"))
                            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                            {
                                csv.WriteRecords(myArmors);
                            }
                            Prompt($"...");
                            Prompt($"Player armor inventory file saved as {player.name}armor.csv\n");

                            // shop weap inv save
                            using (var writer = new StreamWriter($"{player.name}ShopWeaponSave.csv"))
                            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                            {
                                csv.WriteRecords(weaponList);
                            }
                            Prompt($"...");
                            Prompt($"Shop weapon file saved as {player.name}ShopWeaponSave.csv\n");

                            // shop inv armor save
                            using (var writer = new StreamWriter($"{player.name}ShopArmorSave.csv"))
                            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                            {
                                csv.WriteRecords(armorList);
                            }
                            Prompt($"...");
                            Prompt($"Shop armor file saved as {player.name}ShopArmorSave.csv\n");

                            Prompt($"Please come again!");
                            gameRunning = false;
                            break;
                        }
                        else if (saveOrQuit == "n")
                        {
                            Prompt($"\nProgress not saved...");
                            Prompt($"Please come again!");
                            gameRunning = false;
                            break;
                        }
                        break;
                  
                
                              
                    // asks user to enter name for player weap and armor inv load
                    case "load":          
                        bool loading = true;
                        while (loading)
                        {
                            Prompt($"Enter the name belonging to the save file\n");
                            
                            int numberOfTries = 0;
                            int maxNumOfTries = 3;
                            bool weapLoad = true;
                            bool armorLoad = true;

                            // loading a weapon, if no weapon file loaded. exit loop before armor load and print error
                            while (weapLoad)
                            {
                                player.name = Console.ReadLine().Trim();
                                try
                                {
                                    using (var reader = new StreamReader($"{player.name}weap.csv"))
                                    {
                                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                                        {
                                            myWeaps = csv.GetRecords<Weapon.WeaponStruct>().ToList<Weapon.WeaponStruct>();
                                        }
                                    }
                                    Prompt("...");
                                    Prompt($"{player.name}'s weapon inventory file loaded successfully\n");
                                    weapLoad = false;
                                    loading = false;
                                }
                                catch (HeaderValidationException)
                                {
                                    Prompt($"You may have tried loading the wrong inventory file\nMake sure you are loading your WEAPON save file");
                                    if (++numberOfTries == maxNumOfTries)
                                    {
                                        Prompt($"You have reached the maximun amount of tries to load your inventory\n" +
                                            $"Please check your inventory to make sure there is a file to load\n");
                                        loading = false;
                                        armorLoad = false;                                           
                                    }
                                }
                                catch (FileNotFoundException)
                                {
                                    Prompt($"\n*File Not Found*\n");
                                    Prompt($"Check the weapon save file name and try again\n");
                                    if (++numberOfTries == maxNumOfTries)
                                    {
                                        Prompt($"You have reached the maximun amount of tries to load your inventory\n" +
                                            $"Please check your inventory to make sure there is a file to load\n");
                                        armorLoad = false;
                                        loading = false;
                                        break;
                                    }

                                }
                            }
                            
                            // loading armor. if no armor file loaded, clear loaded weapon file, exit loop and print error
                            if (armorLoad)
                            {
                                while (armorLoad)
                                {
                                    try
                                    {
                                        using (var reader = new StreamReader($"{player.name}armor.csv"))
                                        {
                                            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                                            {
                                                myArmors = csv.GetRecords<Armor.ArmorStruct>().ToList<Armor.ArmorStruct>();
                                            }
                                        }
                                        Prompt("...");
                                        Prompt($"{player.name}'s armor file loaded successfully\n");
                                        armorLoad = false;
                                        loading = false;
                                    }
                                    catch (HeaderValidationException)
                                    {
                                        if (++numberOfTries == maxNumOfTries)
                                        {
                                            Prompt($"You have reached the maximun amount of tries to load your inventory\n" +
                                                $"Please check your inventory to make sure there is a file to load\n");
                                                
                                            loading = false;
                                        }
                                        Prompt($"You may have tried loading the wrong file\nMake sure you are loading your ARMOR save file\n");
                                    }
                                    catch (FileNotFoundException)
                                    {
                                        Prompt($"\n*File Not Found*\n");
                                        Prompt($"Check the armor save file name and try again\n");
                                        if (++numberOfTries == maxNumOfTries)
                                        {
                                            myWeaps.Clear();
                                            Prompt($"You have reached the maximun amount of tries to load your inventory\n" +
                                            $"*previously loaded inventory set back to default*\n" +
                                            $"Please check your inventory to make sure there is a file to load\n");
                                            armorLoad = false;
                                            loading = false;                                                
                                        }
                                    }
                                }
                            }
                            
                            // if both weapon and armor files loaded, load the shop inventories
                            // if not found clear both player inventories and exit to main loop
                            if (!loading)
                            {
                                try
                                {
                                    // shop weapon inv load
                                    using (var reader = new StreamReader($"{player.name}ShopWeaponSave.csv"))
                                    {
                                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                                        {
                                            weaponList = csv.GetRecords<Weapon.WeaponStruct>().ToList<Weapon.WeaponStruct>();
                                        }
                                    }
                                    // shop armor inv load
                                    using (var reader = new StreamReader($"{player.name}ShopArmorSave.csv"))
                                    {
                                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                                        {
                                            armorList = csv.GetRecords<Armor.ArmorStruct>().ToList<Armor.ArmorStruct>();
                                        }
                                    }
                                    Prompt($"Loaded player inventory successfully\n" +
                                    $"\nLoaded shop inventories successfully\n");
                                    Prompt($"Welcome back, {player.name}!\n");
                                }
                                catch (FileNotFoundException)
                                {
                                    myArmors.Clear();
                                    myWeaps.Clear();
                                    Prompt($"There was an issue loading the shop inventories\n" +
                                        $"Please make sure there is a save file to load and try again\n" +
                                        $"There is a possibility the last game progress wasnt saved\n");
                                    break;
                                 
                                }                                                              
                            }
                        }
                        break;
                    

                }
                // gets 'a' or 'w' for choosing an item to view
                // removes 'a' or 'w' to get number associated with proper inventory
                if (!inputCommandDealtWith)
                {
                    // error handling for nothing entered to console
                    try
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
                                        $"Please pick again and dont forget to check the weapon list to see what number corresponds to each weapon\n" +
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
                                       $"Please pick again and dont forget to check the armor list to see what number corresponds to each armor\n" +
                                       $"_______________");
                                }
                                break;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Prompt($"\nYou must enter something into the console\nType 'help' for more instructions\n");
                    }
                }
            }

            // keeps console window open after game exit
            Console.ReadKey();
            #endregion
        }

        #region Methods for Showing, Buying and Selling
        // shows all weapon names and prices, using a weapon list
        static public void ShowWeapons(List<Weapon.WeaponStruct> weaponList)
        {            
            Prompt($"\n\n----WEAPONS----\n     vvvvv    \n");
            foreach (Weapon.WeaponStruct tmpWeap in weaponList)
            {
                weaponNumber++;
                Prompt($"(w{weaponNumber})\n" +
                    $"{tmpWeap.Name}\n" +
                    $"-{tmpWeap.Price} units-" +
                    $"\n_______________");
            }
            // resets number so it doesnt duplicate upon multiple entry of show weapons
            weaponNumber = 0;
            Prompt($"\n     ^^^^^    \n----WEAPONS----");

            // if player views weapon with no money, warning will appear
            if (playerBank <= 0)
            {
                playerBank = 0;
                Prompt($"Uh oh! You don't have any units left..\nPlease sell something or come back when you have some more units!");
            }
        }

        //shows all armor names and prices, using and armor list
        static public void ShowArmor(List<Armor.ArmorStruct> armorList)
        {          
            Prompt($"\n\n----ARMOR----\n     vvv    \n");
            foreach (Armor.ArmorStruct tmpArmor in armorList)
            {
                armorNumber++;
                Prompt($"(a{armorNumber})\n" +
                    $"{tmpArmor.Name}\n" +
                    $"-{tmpArmor.Price} units-" +
                    $"\n_______________");

            }
            // resets number so it doesnt duplicate upon multiple entry of show armor
            armorNumber = 0;
            Prompt($"\n     ^^^    \n----ARMOR----");

            // if player views weapon with no money, warning will appear
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
            Item weapon = new Weapon();

            // prints all values using overriden method
            weapon.ToString(tmpWeap.Name, tmpWeap.Type, tmpWeap.Info, tmpWeap.AttackPwr, tmpWeap.Rarity, tmpWeap.Price);

            // no money in player bank warning
            if (playerBank <= 0)
            {
                playerBank = 0;
                Prompt($"Uh oh! You don't have any units left..\nPlease sell something or come back when you have some more units!");
                return;
            }
            Prompt($"Would you like to buy this weapon? ('y' or 'n')");
            string input = Console.ReadLine().ToLower().Trim();
            if (input == "n")
            {
                return;
            }
            if (input != "y")
            {
                Prompt($"Oops! Looks like you entered and invalid response. \nPlease select your desired weapon and try again\n" +
                    $"*Remember*: \nenter 'y' to buy item\nenter 'n' to decline\n___________________");
                return;
            }
            if (input == "y")
            {
                // check for player having enough units to purchase
                if (playerBank < tmpWeap.Price)
                {
                    Prompt($"Hey! You dont have any enough units left to purchase that!");
                    Prompt($"\nYou have {playerBank} units and this item is {tmpWeap.Price}\n");
                    return;
                }
                else
                {
                    weaponList.Remove(tmpWeap);
                    myWeaps.Add(tmpWeap);

                    playerBank -= tmpWeap.Price;
                    shopBank += tmpWeap.Price;

                    Prompt($"\nYou now own {tmpWeap.Name}!\n" +
                        $"\nYou currently have {playerBank} units left in your bank\n________________\n");
                    return;
                }
            }
        }

        // method to view a speciic armor and asks to purchase
        static public void ViewAndBuyArmor(List<Armor.ArmorStruct> armorList, List<Armor.ArmorStruct> myArmors, int indexNum)
        {
            Armor.ArmorStruct tmpArmor = armorList[indexNum - 1];
            Item armor = new Armor();

            armor.ToString(tmpArmor.Name, tmpArmor.Type, tmpArmor.Info, tmpArmor.Defense, tmpArmor.Rarity, tmpArmor.Price);

            if (playerBank <= 0)
            {
                playerBank = 0;
                Prompt($"Uh oh! You don't have any units left..\nPlease sell something or come back when you have some more units!");
                return;
            }

            Prompt($"Would you like to buy this armor? ('y' or 'n')");
            string input = Console.ReadLine().ToLower().Trim(); ;
            if (input == "n")
            {
                return;
            }
            if (input != "y")
            {
                Prompt($"Oops! Looks like you entered and invalid response. \nPlease select your desired armor piece and try again\n" +
                       $"*Remember*: \nenter 'y' or 'Y' to buy item\nenter 'n' or 'N' to decline\n___________________\n");
                return;
            }
            if (input == "y")
            {
                if (playerBank < tmpArmor.Price)
                {
                    Prompt($"\nHey! You dont have any enough units left to purchase that!");
                    Prompt($"\nYou have {playerBank} units and this item is {tmpArmor.Price}\n");
                    return;
                }
                else
                {
                    armorList.Remove(tmpArmor);
                    myArmors.Add(tmpArmor);

                    playerBank -= tmpArmor.Price;
                    shopBank += tmpArmor.Price;

                    Prompt($"\nYou now own {tmpArmor.Name}!\n" +
                        $"\nYou currently have {playerBank} units left in your bank\n___________________\n");
                    return;
                }
            }
        }

        // method to sell a specific weapon in player inventory
        static public void SellWeap(List<Weapon.WeaponStruct> myWeapInv, List<Weapon.WeaponStruct> weaponList, int indexNum)
        {
            Weapon.WeaponStruct tmpWeap = myWeapInv[indexNum - 1];
            Item weapon = new Weapon();

            // prints all values using overriden method
            weapon.ToString(tmpWeap.Name, tmpWeap.Type, tmpWeap.Info, tmpWeap.AttackPwr, tmpWeap.Rarity, tmpWeap.Price);

            Prompt($"Would you like to sell this item? ('y' or 'n')");
            string input = Console.ReadLine().ToLower().Trim();

            if (input == "n")
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
            if (input == "y")
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
                    Prompt($"The shop bank is now at {shopBank}\n");
                }
            }
        }

        // method to sell a specific armor
        static public void SellArmor(List<Armor.ArmorStruct> myArmorInv, List<Armor.ArmorStruct> armorList, int indexNum)
        {
            Armor.ArmorStruct tmpArmor = myArmorInv[indexNum - 1];
            Item armor = new Armor();

            // prints values using overriden method
            armor.ToString(tmpArmor.Name, tmpArmor.Type, tmpArmor.Info, tmpArmor.Defense, tmpArmor.Rarity, tmpArmor.Price);

            Prompt($"Would you like to sell this item? ('y' or 'n')");
            string input = Console.ReadLine().Trim().ToLower();
            if (input == "n")
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
            if (input == "y")
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
            Item weapon = new Weapon();
            int numOfWeap = 0;
            Prompt($"\n----MY WEAPONS----\n     vvvvvvvv    \n");
            foreach (Weapon.WeaponStruct myInv in myWeaps)
            {
                numOfWeap++;
                Prompt($"Weapon (w{numOfWeap})");
                weapon.ToString(myInv.Name, myInv.Type, myInv.Info, myInv.AttackPwr, myInv.Rarity, myInv.Price);
            }
            Prompt($"\n     ^^^^^^^^    \n----MY WEAPONS----\n");
        }

        // shows all information of armor in player inv
        static public void PlayerArmorInv(List<Armor.ArmorStruct> myArmors)
        {
            Item armor = new Armor();
            int numOfArmor = 0;
            Prompt($"\n----MY ARMOR----\n     vvvvvv    \n");
            foreach (Armor.ArmorStruct myInv in myArmors)
            {
                numOfArmor++;
                Prompt($"Armor (a{numOfArmor})");
                armor.ToString(myInv.Name, myInv.Type, myInv.Info, myInv.Defense, myInv.Rarity, myInv.Price);
            }           
            Prompt($"\n     ^^^^^^    \n----MY ARMOR----\n");
        }
        #endregion

        // simple method for writing prompts
        static public void Prompt(string prompt)
        {
            Console.WriteLine(prompt);
        }
    }
}
