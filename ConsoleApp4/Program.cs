using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

// Klasa główna SmartHome
class SmartHome
{
    // Ogólne właściwości i metody inteligentnego domu
    public string HomeName { get; set; }
    public bool IsSecuritySystemEnabled { get; set; }
    private AccessControl accessControl;
    private FaultMessage faultMessage;
    private SecurityMonitoring securityMonitoring;
    private EnvironmentalMonitoring environmentalMonitoring;
    private LightingAndEnergyControl lightingAndEnergyControl;
    private RemoteBuildingControl remoteBuildingControl;
    private bool isUserLoggedIn = false;

    public SmartHome(string homeName, AccessControl accessControl, FaultMessage faultMessage)
    {
        HomeName = homeName;
        IsSecuritySystemEnabled = false;
        this.accessControl = accessControl;
        this.faultMessage = faultMessage;
    }

    public void TurnOnSecuritySystem()
    {
        IsSecuritySystemEnabled = true;
        Console.WriteLine("Security system is now enabled.");
    }

    public void TurnOffSecuritySystem()
    {
        IsSecuritySystemEnabled = false;
        Console.WriteLine("Security system is now disabled.");
    }

    public virtual void MainFunction()
    {
        Console.WriteLine("Performing main function of the SmartHome.");
    }

    // Menu główne
    public void MainMenu()
    {
        while (true)
        {
            Console.WriteLine("Menu główne:");

            if (!isUserLoggedIn)
            {
                Console.WriteLine("1. Dodaj użytkownika");
                Console.WriteLine("2. Zaloguj się");
                Console.WriteLine("3. Wyloguj się");
            }
            else
            {
                Console.WriteLine("1. Kontrola dostępu");
                Console.WriteLine("2. Zgłaszanie usterek");

                if (securityMonitoring == null)
                {
                    Console.WriteLine("3. Monitorowanie bezpieczeństwa");
                }
                else
                {
                    Console.WriteLine("3. Włącz/wyłącz kamery (monitorowanie bezpieczeństwa)");
                }

                if (environmentalMonitoring == null)
                {
                    Console.WriteLine("4. Monitorowanie srodowiskowe");
                }
                else
                {
                    Console.WriteLine("4. Wyświetlanie temperatury wody i powietrza (monitorowanie środowiska)");
                }

                if (lightingAndEnergyControl == null)
                {
                    Console.WriteLine("5. Zarzadzanie oswietleniem i energia");
                }
                else
                {
                    Console.WriteLine("5. Włącz/wyłącz automatyczne sterowanie oświetleniem (zarządzanie oświetleniem i energią)");
                }

                if (remoteBuildingControl == null)
                {
                    Console.WriteLine("6. Zdalne sterowanie budynkiem");
                }
                else
                {
                    Console.WriteLine("6. Włącz/wyłącz zdalne sterowanie (Zdalne sterowanie budynkiem)");
                }

                Console.WriteLine("7. Wyloguj się");
            }

            Console.Write("Wybierz akcję: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (!isUserLoggedIn)
                    {
                        accessControl.AddNewUser();
                    }
                    else
                    {
                        AccessControlMenu();
                    }
                    break;
                case "2":
                    if (!isUserLoggedIn)
                    {
                        isUserLoggedIn = accessControl.Login();
                    }
                    else
                    {
                        FaultMessageMenu();
                    }
                    break;
                case "3":
                    if (!isUserLoggedIn)
                    {
                        Console.WriteLine("Wyloguj się.");
                        return;
                    }
                    else
                    {
                        if (securityMonitoring == null)
                        {
                            securityMonitoring = new SecurityMonitoring(HomeName, accessControl, faultMessage);
                        }

                        SecurityMonitoringMenu();
                    }
                    break;
                case "4":
                    if (!isUserLoggedIn)
                    {
                        Console.WriteLine("Wyloguj się.");
                        return;
                    }
                    else
                    {
                        if (environmentalMonitoring == null)
                        {
                            environmentalMonitoring = new EnvironmentalMonitoring(HomeName, accessControl, faultMessage);
                        }

                        EnvironmentalMonitoringMenu();
                    }
                    break;
                case "5":
                    if (!isUserLoggedIn)
                    {
                        Console.WriteLine("Wyloguj się.");
                        return;
                    }
                    else
                    {
                        if (lightingAndEnergyControl == null)
                        {
                            lightingAndEnergyControl = new LightingAndEnergyControl(HomeName, accessControl, faultMessage);
                        }

                        LightingAndEnergyControlMenu();
                    }
                    break;
                case "6":
                    if (!isUserLoggedIn)
                    {
                        Console.WriteLine("Wyloguj się.");
                        return;
                    }
                    else
                    {
                        if (remoteBuildingControl == null)
                        {
                            remoteBuildingControl = new RemoteBuildingControl(HomeName, accessControl, faultMessage);
                        }

                        RemoteBuildingControlMenu();
                    }
                    break;
                case "7":
                    if (isUserLoggedIn)
                    {
                        Console.WriteLine("Wyloguj się.");
                        isUserLoggedIn = false;
                    }
                    break;
                default:
                    Console.WriteLine("Zły wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    // Menu kontroli dostępu
    private void AccessControlMenu()
    {
        while (true)
        {
            Console.WriteLine("Menu kontroli dostępu:");
            Console.WriteLine("1. Usuń użytkownika");
            Console.WriteLine("2. Zobacz historię logowania");
            Console.WriteLine("3. Zmień dane użytkownika");
            Console.WriteLine("4. Wyświetl wszystkich użytkowników");
            Console.WriteLine("5. Wyczyść historię logowania"); 
            Console.WriteLine("6. Powrót do menu głównego");

            Console.Write("Wybierz akcję: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    accessControl.RemoveUser();
                    break;
                case "2":
                    accessControl.ViewLoginHistory();
                    break;
                case "3":
                    accessControl.ChangeUserData();
                    break;
                case "4":
                    accessControl.ViewAllUsers();
                    break;
                case "5":
                    accessControl.ClearLoginHistory();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Zły wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    // Menu "zgłaszanie usterek"
    private void FaultMessageMenu()
    {
        while (true)
        {
            Console.WriteLine("Menu zgłaszanie usterek:");
            Console.WriteLine("1. Zobacz historię usterek");
            Console.WriteLine("2. Prześlij raport o problemie");
            Console.WriteLine("3. Zmień status naprawy");
            Console.WriteLine("4. Wyczyść historię usterek");
            Console.WriteLine("5. Powrót do menu głównego");

            Console.Write("Wybierz akcję: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    faultMessage.ShowMessages();
                    break;
                case "2":
                    Console.Write("Opisz problem: ");
                    string newMessage = Console.ReadLine();
                    Console.Write("Wprowadz status naprawy (na przyklad W toku): ");
                    string repairStatus = Console.ReadLine();
                    faultMessage.AddMessage(newMessage, repairStatus, accessControl.CurrentUserFirstName, accessControl.CurrentUserLastName);
                    Console.WriteLine("Wiadomość została dodana pomyślnie.");
                    break;
                case "3":
                    faultMessage.ChangeRepairStatus();
                    break;
                case "4":
                    faultMessage.ClearMessageHistory();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Zły wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    // Menu "Monitorowanie bezpieczenstwa"
    private void SecurityMonitoringMenu()
    {
        while (true)
        {
            Console.WriteLine("Menu Monitoring bezpieczenstwa:");

            if (securityMonitoring.AreCamerasEnabled)
            {
                Console.WriteLine("1. Włącz/wyłącz kamery");
                Console.WriteLine("2. Zobacz wideo z kamery");
                Console.WriteLine("3. Aktywuj alarm");
                Console.WriteLine("4. Wyswietl historie alarmow");
                Console.WriteLine("5. Powrót do menu głównego");
            }
            else
            {
                Console.WriteLine("1. Włącz/wyłącz kamery");
                Console.WriteLine("2. Zobacz wideo z kamery");
                Console.WriteLine("3. Aktywuj alarm");
                Console.WriteLine("4. Wyswietl historie alarmow");
                Console.WriteLine("5. Powrót do menu głównego");
            }

            Console.Write("Wybierz akcję: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    securityMonitoring.ToggleCameras();
                    break;
                case "2":
                    securityMonitoring.ViewCameraVideo();
                    break;
                case "3":
                    securityMonitoring.ReportSuspiciousActivity();
                    break;
                case "4":
                    securityMonitoring.ViewSuspiciousActivityHistory();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Zły wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }


    // Menu "Monitorowanie srodowiskowe"
    private void EnvironmentalMonitoringMenu()
    {
        while (true)
        {
            Console.WriteLine("Menu Monitorowanie srodowiskowe:");
            Console.WriteLine("1. Zobacz temperaturę wody");
            Console.WriteLine("2. Wyświetlanie temperatury powietrza");
            Console.WriteLine("3. Automatyczne sterowanie systemami klimatyzacji i ogrzewania");
            Console.WriteLine("4. Zmien temperatury progowe dla ogrzewania i klimatyzacji");
            Console.WriteLine("5. Wyświetl stan ogrzewania i klimatyzacji");
            Console.WriteLine("6. Zglaszaj mozliwe awarie w systemach srodowiskowych");
            Console.WriteLine("7. Przegladaj historie awarii w systemie ekologicznym");
            Console.WriteLine("8. Powrót do menu głównego");

            Console.Write("Wybierz akcję: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    environmentalMonitoring.ViewWaterTemperature();
                    break;
                case "2":
                    environmentalMonitoring.ViewAirTemperature();
                    break;
                case "3":
                    environmentalMonitoring.AutomaticTemperatureControl();
                    break;
                case "4":
                    environmentalMonitoring.SetTemperatureThresholds();
                    break;
                case "5":
                    environmentalMonitoring.ViewHeatingCoolingStatus();
                    break;
                case "6":
                    environmentalMonitoring.WriteEnvironmentalFailureMessage();
                    break;
                case "7":
                    environmentalMonitoring.ViewEnvironmentalFailureHistory();
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Zły wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    // Menu "Zarzadzanie oswietleniem i energia"
    private void LightingAndEnergyControlMenu()
    {
        while (true)
        {
            Console.WriteLine("Menu Zarzadzanie oswietleniem i energia:");
            Console.WriteLine("1. Włącz automatyczne sterowanie oświetleniem");
            Console.WriteLine("2. Wyłącz automatyczne sterowanie oświetleniem");
            Console.WriteLine("3. Automatyczne sterowanie oswietleniem zgodnie z harmonogramem");
            Console.WriteLine("4. Ustaw zakres czasowy automatycznego sterowania oswietleniem");
            Console.WriteLine("5. Sprawdź i dostosuj harmonogram oświetlenia");
            Console.WriteLine("6. Zapisz raport stanu energii");
            Console.WriteLine("7. Przeglądaj raporty stanu energii");
            Console.WriteLine("8. Powrót do menu głównego");

            Console.Write("Wybierz akcję: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    lightingAndEnergyControl.TurnOnAutomaticLightingControl();
                    break;
                case "2":
                    lightingAndEnergyControl.TurnOffAutomaticLightingControl();
                    break;
                case "3":
                    lightingAndEnergyControl.AutomaticLightingControlBasedOnTime();
                    break;
                case "4":
                    lightingAndEnergyControl.SetCustomLightingSchedule();
                    break;
                case "5":
                    lightingAndEnergyControl.CheckAndAdjustLightingSchedule();
                    break;
                case "6":
                    lightingAndEnergyControl.RecordEnergyConsumptionReport();
                    break;
                case "7":
                    lightingAndEnergyControl.ViewEnergyConsumptionReports();
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Zły wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    // Menu "Zdalne sterowanie budynkiem"
    private void RemoteBuildingControlMenu()
    {
        while (true)
        {
            Console.WriteLine("Menu Zdalne sterowanie budynkiem:");
            Console.WriteLine("1. Włącz zdalne sterowanie");
            Console.WriteLine("2. Wyłącz zdalne sterowanie");
            Console.WriteLine("3. Wykonaj zdalne sterowanie");
            Console.WriteLine("4. Wyświetl historię zdalnego sterowania");
            Console.WriteLine("5. Powrót do menu głównego");

            Console.Write("Wybierz akcję: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    remoteBuildingControl.TurnOnRemoteControl();
                    break;
                case "2":
                    remoteBuildingControl.TurnOffRemoteControl();
                    break;
                case "3":
                    remoteBuildingControl.RemoteControlFunction();
                    break;
                case "4":
                    remoteBuildingControl.ViewRemoteControlHistory();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Zły wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }
}

// Klasa AccessControl współpracująca z SmartHome
class AccessControl : SmartHome
{
    // Dodatkowe właściwości i metody kontroli dostępu
    public string AccessCode { get; set; }
    public string CurrentUserFirstName { get; private set; }
    public string CurrentUserLastName { get; private set; }

    public AccessControl(string homeName, string accessCode) : base(homeName, null, null)
    {
        AccessCode = accessCode;
    }

    public override void MainFunction()
    {
        base.MainFunction();
        Console.WriteLine("Performing access control function.");
    }

    // Add new user function
    public void AddNewUser()
    {
        Console.Write("Wpisz swoje imię: ");
        string firstName = Console.ReadLine();

        Console.Write("Wpisz nazwisko: ");
        string lastName = Console.ReadLine();

        Console.Write("Wpisz Twój kod PIN: ");
        string pinCode = Console.ReadLine();

        // Dodawanie informacji do pliku tekstowego
        string userInformation = $"{firstName} {lastName} {pinCode}";
        File.AppendAllText("users.txt", userInformation + Environment.NewLine);
        Console.WriteLine("Nowy użytkownik został pomyślnie dodany.");
    }

    // Funkcja logowania
    public bool Login()
    {
        Console.Write("Wpisz swój kod PIN, aby się zalogować: ");
        string enteredPin = Console.ReadLine();

        // Odczytujemy piny z pliku i sprawdzamy obecność wprowadzonego pinu
        string[] pins = File.ReadAllLines("users.txt");
        bool pinFound = false;

        foreach (string pin in pins)
        {
            if (pin.EndsWith(enteredPin))
            {
                // Pobieranie imienia i nazwiska użytkownika
                string[] userInfo = pin.Split(' ');
                CurrentUserFirstName = userInfo[0];
                CurrentUserLastName = userInfo[1];

                pinFound = true;

                // Zapisujemy informacje do pliku logowania
                string loginHistory = $"{DateTime.Now}: Zalogowany - {CurrentUserFirstName} {CurrentUserLastName}";
                File.AppendAllText("login_history.txt", loginHistory + Environment.NewLine);

                break;
            }
        }

        if (pinFound)
        {
            Console.WriteLine($"Dostęp jest dozwolony. Logowanie zakończone sukcesem, {CurrentUserFirstName} {CurrentUserLastName}.");
            return true;
        }
        else
        {
            Console.WriteLine("Logowanie nie powiodło się. Nie znaleziono kodu PIN.");
            return false;
        }
    }

    // Delete user function
    public void RemoveUser()
    {
        Console.Write("Wpisz kod PIN użytkownika, którego chcesz usunąć: ");
        string pinToRemove = Console.ReadLine();

        // Odczytanie pinów z pliku i usunięcie informacji o użytkowniku z wprowadzonym pinem
        string[] pins = File.ReadAllLines("users.txt");
        File.WriteAllText("users.txt", string.Empty);

        foreach (string pin in pins)
        {
            if (!pin.EndsWith(pinToRemove))
            {
                File.AppendAllText("users.txt", pin + Environment.NewLine);
            }
        }

        Console.WriteLine("Użytkownik został pomyślnie usunięty z systemu.");
    }

    // Sposób przeglądania i zapisywania historii logowania
    public void ViewLoginHistory()
    {
        Console.WriteLine("Historia logowania:");

        // Przeczytaj logi z pliku i wyślij je do konsoli
        string[] loginHistory = File.ReadAllLines("login_history.txt");

        if (loginHistory.Length == 0)
        {
            Console.WriteLine("Brak wpisów w historii logowania.");
        }
        else
        {
            foreach (var login in loginHistory)
            {
                Console.WriteLine(login);
            }
        }
    }

    // Sposób zmiany danych użytkownika
    public void ChangeUserData()
    {
        Console.WriteLine("Zmiana danych użytkownika:");

        // Wyświetla aktualne informacje o użytkowniku
        Console.WriteLine($"Aktualne imię: {CurrentUserFirstName}");
        Console.WriteLine($"Aktualne nazwisko: {CurrentUserLastName}");
        Console.WriteLine($"Aktualny PIN: {AccessCode}");

        // Prośba o nowe dane
        Console.Write("Wprowadź nową imię: ");
        string newFirstName = Console.ReadLine();

        Console.Write("Wprowadź nowe nazwisko: ");
        string newLastName = Console.ReadLine();

        Console.Write("Wpisz swój nowy PIN: ");
        string newPinCode = Console.ReadLine();

        // Aktualizacja danych użytkownika
        CurrentUserFirstName = newFirstName;
        CurrentUserLastName = newLastName;
        AccessCode = newPinCode;

        // Aktualizujemy informacje w pliku z użytkownikami
        UpdateUserInfoInFile();

        Console.WriteLine("Dane użytkownika zostały pomyślnie zmienione.");
    }

    // Metoda aktualizacji informacji o użytkowniku w pliku
    private void UpdateUserInfoInFile()
    {
        string[] pins = File.ReadAllLines("users.txt");
        File.WriteAllText("users.txt", string.Empty);

        foreach (string pin in pins)
        {
            if (!pin.EndsWith(AccessCode))
            {
                File.AppendAllText("users.txt", pin + Environment.NewLine);
            }
        }

        // Dodawanie aktualnych informacji
        string userInformation = $"{CurrentUserFirstName} {CurrentUserLastName} {AccessCode}";
        File.AppendAllText("users.txt", userInformation + Environment.NewLine);
    }

    // Metoda przeglądania wszystkich użytkowników
    public void ViewAllUsers()
    {
        Console.WriteLine("Lista wszystkich użytkowników:");

        // Odczytaj informacje o użytkownikach z pliku i wyślij je do konsoli
        string[] users = File.ReadAllLines("users.txt");

        if (users.Length == 0)
        {
            Console.WriteLine("Brak zarejestrowanych użytkowników.");
        }
        else
        {
            foreach (var user in users)
            {
                Console.WriteLine(user);
            }
        }
    }

    // Metoda czyszczenia historii logowań
    public void ClearLoginHistory()
    {
        Console.WriteLine("Czyszczenie historii logowania...");

        // Czyszczenie pliku historii logowania
        File.WriteAllText("login_history.txt", string.Empty);

        Console.WriteLine("Historia logowania została pomyślnie wyczyszczona.");
    }
}

// Klasa FaultMessage odziedziczona z SmartHome
class FaultMessage : SmartHome
{
    // Lista komunikatów o błędach
    private List<string> messages;

    public FaultMessage(string homeName) : base(homeName, null, null)
    {
        messages = new List<string>();
        LoadMessages();
    }

    // Ładowanie komunikatów z pliku podczas tworzenia obiektu
    private void LoadMessages()
    {
        if (File.Exists("fault_messages.txt"))
        {
            messages = new List<string>(File.ReadAllLines("fault_messages.txt"));
        }
    }

    // Metoda aktualizacji wiadomości w pliku
    private void UpdateMessagesInFile()
    {
        File.WriteAllLines("fault_messages.txt", messages);
    }

    // Metoda dodawania komunikatu o usterce i statusu naprawy
    public void AddMessage(string message, string repairStatus, string userFirstName, string userLastName)
    {
        string fullMessage = $"{userFirstName} {userLastName}: {message} (Stan naprawy: {repairStatus})";
        messages.Add(fullMessage);

        // Informacje zapisujemy do pliku tekstowego
        UpdateMessagesInFile();

        // Zapisywanie numerów sekwencyjnych do pliku
        SaveMessageIndexesToFile();

        Console.WriteLine("Wiadomość została dodana pomyślnie. Numer wiadomości: " + (messages.Count - 1));
    }

    // Metoda zmiany statusu naprawy
    public void ChangeRepairStatus()
    {
        Console.WriteLine("Zmiana statusu naprawy:");

        // Pokaż aktualne komunikaty o błędach
        ShowMessages();

        Console.Write("Wpisz numer seryjny wiadomości, której status naprawy chcesz zmienić: ");
        if (int.TryParse(Console.ReadLine(), out int messageIndex) && messageIndex >= 0 && messageIndex < messages.Count)
        {
            Console.Write("Podaj nowy status naprawy: ");
            string newRepairStatus = Console.ReadLine();

            // Wiadomość dzielimy na imię, nazwisko, opis i status naprawy
            string[] messageParts = messages[messageIndex].Split(':');
            string repairStatusPart = messageParts.Length > 1 ? messageParts[1] : "";

            // Ponowne składanie komunikatu z nowym statusem naprawy
            string updatedMessage = $"{messageParts[0]}:{newRepairStatus}";
            messages[messageIndex] = updatedMessage;

            // Zapisz zaktualizowane wiadomości do pliku
            UpdateMessagesInFile();

            Console.WriteLine("Status naprawy został pomyślnie zmieniony.");
        }
        else
        {
            Console.WriteLine("Nieprawidłowy numer wiadomości. Spróbuj ponownie.");
        }
    }

    // Metoda zapisywania numerów sekwencyjnych do pliku
    private void SaveMessageIndexesToFile()
    {
        string[] indexes = new string[messages.Count];

        for (int i = 0; i < messages.Count; i++)
        {
            indexes[i] = i.ToString();
        }

        File.WriteAllLines("message_indexes.txt", indexes);
    }

    // Metoda wyświetlania wszystkich Wiadomości
    public void ShowMessages()
    {
        Console.WriteLine("Wiadomości o błędach:");

        if (messages.Count == 0)
        {
            Console.WriteLine("Brak wiadomości.");
        }
        else
        {
            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
        }
    }

    // Metoda czyszczenia całej historii wiadomości
    public void ClearMessageHistory()
    {
        Console.WriteLine("Czyszczenie historii wiadomości...");

        // Czyszczenie pliku historii wiadomości
        messages.Clear();

        // Czyszczenie pliku historii wiadomości
        File.WriteAllText("fault_messages.txt", string.Empty);

        Console.WriteLine("Historia wiadomości została pomyślnie wyczyszczona.");
    }
}
// Klasa SecurityMonitoring
class SecurityMonitoring : SmartHome
{
    private bool areCamerasEnabled;

    // Dodano nowy plik tekstowy do rejestrowania wiadomości o podejrzanej aktywności
    private const string SuspiciousActivityLogFile = "suspicious_activity_log.txt";

    public SecurityMonitoring(string homeName, AccessControl accessControl, FaultMessage faultMessage)
        : base(homeName, accessControl, faultMessage)
    {
        areCamerasEnabled = false;
    }

    // Właściwość sprawdzania stanu kamery
    public bool AreCamerasEnabled => areCamerasEnabled;

    // Funkcja włączania/wyłączania kamer
    public void ToggleCameras()
    {
        areCamerasEnabled = !areCamerasEnabled;
        Console.WriteLine($"Kamery już są {(areCamerasEnabled ? "włączony" : "wyłączony")}.");
    }

    // Funkcja przeglądania obrazu z kamer
    public void ViewCameraVideo()
    {
        if (areCamerasEnabled)
        {
            Console.WriteLine("Oglądanie obrazu na żywo z kamer.");
        }
        else
        {
            Console.WriteLine("Kamery nie są włączone. Najpierw włącz kamery.");
        }
    }

    // Metoda zgłaszania podejrzanej aktywności
    public void ReportSuspiciousActivity()
    {
        Console.Write("Podaj lokalizację w domu, w której wykryto podejrzaną aktywność ");
        string location = Console.ReadLine();

        Console.Write("Opisz podejrzaną aktywność: ");
        string description = Console.ReadLine();

        // Zapisywanie informacji do pliku
        string logEntry = $"{DateTime.Now}: Podejrzana aktywność w {location} - {description}";
        File.AppendAllText(SuspiciousActivityLogFile, logEntry + Environment.NewLine);

        Console.WriteLine("Informacja o podejrzanej aktywności została zapisana.");
    }

    // Metoda przeglądania historii raportów o podejrzanej aktywności
    public void ViewSuspiciousActivityHistory()
    {
        Console.WriteLine("Historie alarmow:");

        // Przeczytaj logi z pliku i wyślij je do konsoli
        string[] activityHistory = File.ReadAllLines(SuspiciousActivityLogFile);

        if (activityHistory.Length == 0)
        {
            Console.WriteLine("Nie ma historii alarmow.");
        }
        else
        {
            foreach (var activity in activityHistory)
            {
                Console.WriteLine(activity);
            }
        }
    }


    // Zastępowanie metody w celu uzyskania dodatkowej funkcjonalności
    public override void MainFunction()
    {
        base.MainFunction();
        Console.WriteLine("Performing security monitoring function.");
    }
}

// Monitoring środowiska nowej klasy
class EnvironmentalMonitoring : SmartHome
{
    private float waterTemperature;
    private float airTemperature;

    // Właściwości umożliwiające monitorowanie stanu ogrzewania i klimatyzacji
    public bool HeatingSystemEnabled { get; private set; }
    public bool CoolingSystemEnabled { get; private set; }

    // Nowy plik tekstowy do nagrywania komunikatów o awariach w systemach środowiskowych
    private const string EnvironmentalFailuresLogFile = "environmental_failures_log.txt";

    // Właściwość do przechowywania komunikatów o awariach
    public List<string> EnvironmentalFailures { get; private set; }

    public EnvironmentalMonitoring(string homeName, AccessControl accessControl, FaultMessage faultMessage)
        : base(homeName, accessControl, faultMessage)
    {
        waterTemperature = 0.0f;
        airTemperature = 20.0f;
        EnvironmentalFailures = new List<string>();
        LoadEnvironmentalFailureHistory();
    }

    // Funkcja sprawdzania temperatury wody
    public void ViewWaterTemperature()
    {
        Console.WriteLine($"Temperatura wody: {waterTemperature} stopnie Celsjusza");
    }

    // Funkcja podglądu temperatury powietrza
    public void ViewAirTemperature()
    {
        Console.WriteLine($"Temperatura powietrza: {airTemperature} stopnie Celsjusza");
    }

    // Sposób automatycznego sterowania instalacjami klimatyzacyjnymi i grzewczymi
    public void AutomaticTemperatureControl()
    {
        if (airTemperature < HeatingThreshold)
        {
            Console.WriteLine("Włączanie ogrzewania...");
        }
        else if (airTemperature > CoolingThreshold)
        {
            Console.WriteLine("Włączam klimatyzację...");
        }
        else
        {
            Console.WriteLine("Temperatura mieści się w normalnych granicach.");
        }
    }

    /// Metoda ustawiania niestandardowych progów temperatury
    public void SetTemperatureThresholds()
    {
        Console.Write("Wprowadź próg temperatury, aby włączyć ogrzewanie: ");
        if (float.TryParse(Console.ReadLine(), out float heatingThreshold))
        {
            Console.Write("Wprowadź próg temperatury, aby włączyć klimatyzację: ");
            if (float.TryParse(Console.ReadLine(), out float coolingThreshold))
            {
                HeatingThreshold = heatingThreshold;
                CoolingThreshold = coolingThreshold;

                // Po aktualizacji wartości progowych sprawdzamy, czy systemy wymagają włączenia/wyłączenia
                AutomaticTemperatureControl();

                // Aktualizujemy stan instalacji grzewczych i klimatyzacyjnych
                UpdateHeatingCoolingStatus();

                Console.WriteLine("Progi temperatury zostały pomyślnie zaktualizowane.");
            }
            else
            {
                Console.WriteLine("Nieprawidłowa wartość progu warunkowania.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowa wartość progu grzania.");
        }
    }

    // Sposób aktualizacji stanu instalacji grzewczych i klimatyzacyjnych
    private void UpdateHeatingCoolingStatus()
    {
        HeatingSystemEnabled = airTemperature < HeatingThreshold;
        CoolingSystemEnabled = airTemperature > CoolingThreshold;
    }

    // Właściwości przechowywania wartości progowych
    public float HeatingThreshold { get; private set; }
    public float CoolingThreshold { get; private set; }

    // Metoda przeglądania stanu ogrzewania i klimatyzacji
    public void ViewHeatingCoolingStatus()
    {
        Console.WriteLine($"Ogrzewanie {(HeatingSystemEnabled ? "dołączony" : "wyłączony")}");
        Console.WriteLine($"Kondycjonowanie {(CoolingSystemEnabled ? "dołączony" : "wyłączony")}");
    }

    // Metoda pisania komunikatu o awarii i zapisywania go do pliku
    public void WriteEnvironmentalFailureMessage()
    {
        Console.Write("Wpisz wiadomość o możliwych awariach w systemach środowiskowych: ");
        string failureMessage = Console.ReadLine();

        // Zapisanie wiadomości na listę i do pliku
        EnvironmentalFailures.Add(failureMessage);
        File.AppendAllText(EnvironmentalFailuresLogFile, $"{DateTime.Now}: {failureMessage}" + Environment.NewLine);

        Console.WriteLine("Wiadomość została pomyślnie zapisana.");
    }

    // Metoda przeglądania historii wiadomości
    public void ViewEnvironmentalFailureHistory()
    {
        Console.WriteLine("Historia wiadomości o możliwych awariach w systemach środowiskowych:");

        if (EnvironmentalFailures.Count == 0)
        {
            Console.WriteLine("W historii wiadomości nie ma żadnych wpisów.");
        }
        else
        {
            foreach (var failureMessage in EnvironmentalFailures)
            {
                Console.WriteLine(failureMessage);
            }
        }
    }

    // Metoda ładowania historii wiadomości z pliku
    private void LoadEnvironmentalFailureHistory()
    {
        if (File.Exists(EnvironmentalFailuresLogFile))
        {
            EnvironmentalFailures = File.ReadAllLines(EnvironmentalFailuresLogFile).ToList();
        }
    }

    // Zastępowanie metody w celu uzyskania dodatkowej funkcjonalności
    public override void MainFunction()
    {
        base.MainFunction();
        AutomaticTemperatureControl();
        Console.WriteLine("Performing environmental monitoring function.");
    }
}

// Nowa klasa Zarządzanie oświetleniem i energią
class LightingAndEnergyControl : SmartHome
{
    private bool automaticLightingControl;
    private float currentEnergyConsumption;
    private List<EnergyConsumptionReport> energyConsumptionReports;

    private TimeSpan customStartTime;
    private TimeSpan customEndTime;

    public LightingAndEnergyControl(string homeName, AccessControl accessControl, FaultMessage faultMessage)
        : base(homeName, accessControl, faultMessage)
    {
        automaticLightingControl = false;
        energyConsumptionReports = new List<EnergyConsumptionReport>();
    }

    // Funkcja umożliwiająca automatyczne sterowanie oświetleniem
    public void TurnOnAutomaticLightingControl()
    {
        automaticLightingControl = true;
        Console.WriteLine("Automatyczne sterowanie oświetleniem jest teraz włączone.");
    }

    // Funkcja wyłączania automatycznego sterowania oświetleniem
    public void TurnOffAutomaticLightingControl()
    {
        automaticLightingControl = false;
        Console.WriteLine("Automatyczne sterowanie oświetleniem jest teraz wyłączone.");
    }

    // Sposób automatycznego włączania świateł w zależności od czasu
    public void AutomaticLightingControlBasedOnTime()
    {
        if (automaticLightingControl)
        {
            // Uzyskiwanie aktualnego czasu
            DateTime currentTime = DateTime.Now;
            int currentHour = currentTime.Hour;

            // Ustaw przedział czasowy automatycznego włączania oświetlenia (na przykład od 18:00 do 6:00)
            int startHour = 18;
            int endHour = 6;

            // Sprawdzamy, czy aktualny czas mieści się w przedziale automatycznego włączenia oświetlenia
            if ((currentHour >= startHour && currentHour <= 23) || (currentHour >= 0 && currentHour < endHour))
            {
                Console.WriteLine("Oświetlenie włącza się automatycznie.");
            }
            else
            {
                Console.WriteLine("Światła są wyłączone.");
            }
        }
        else
        {
            Console.WriteLine("Automatyczne sterowanie oświetleniem jest wyłączone. Światła nie włączą się automatycznie.");
        }
    }
    // Metoda ustawiania niestandardowego czasu włączania i wyłączania oświetlenia
    public void SetCustomLightingSchedule()
    {
        Console.Write("Wprowadź godzinę rozpoczęcia automatycznego włączania oświetlenia (gg:mm): ");
        string startTimeInput = Console.ReadLine();

        Console.Write("Wprowadź godzinę zakończenia automatycznego włączania oświetlenia (gg:mm): ");
        string endTimeInput = Console.ReadLine();

        if (TimeSpan.TryParseExact(startTimeInput, "HH:mm", CultureInfo.InvariantCulture, out TimeSpan startTime) &&
            TimeSpan.TryParseExact(endTimeInput, "HH:mm", CultureInfo.InvariantCulture, out TimeSpan endTime))
        {
            // Zapisujemy czas wpisany przez użytkownika na włączenie i wyłączenie oświetlenia
            customStartTime = startTime;
            customEndTime = endTime;

            Console.WriteLine("Niestandardowy czas włączenia i wyłączenia oświetlenia został pomyślnie ustawiony.");
        }
        else
        {
            Console.WriteLine("Nieprawidłowy format godziny. Spróbuj ponownie.");
        }
    }

    // Sposób sprawdzania i automatycznego korygowania czasu włączania i wyłączania oświetlenia
    public void CheckAndAdjustLightingSchedule()
    {
        if (automaticLightingControl)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan current = currentTime.TimeOfDay;

            if (current < customStartTime || current > customEndTime)
            {
                Console.WriteLine("Oświetlenie powinno być w tym czasie wyłączone.");
            }
            else
            {
                Console.WriteLine("W tym momencie oświetlenie powinno być włączone.");
            }
        }
        else
        {
            Console.WriteLine("Automatyczne sterowanie oświetleniem jest wyłączone. Światła nie włączą się automatycznie.");
        }
    }

    // Klasa wewnętrzna do raportowania zużycia energii
    private class EnergyConsumptionReport
    {
        public DateTime Date { get; set; }
        public float Consumption { get; set; }

        public EnergyConsumptionReport(DateTime date, float consumption)
        {
            Date = date;
            Consumption = consumption;
        }

        public override string ToString()
        {
            return $"{Date}: Pobór energii - {Consumption} kWh";
        }
    }

    // Sposób rejestrowania raportu stanu energii
    public void RecordEnergyConsumptionReport()
    {
        if (automaticLightingControl)
        {
            Random random = new Random();
            float consumption = random.Next(50, 200);
            currentEnergyConsumption += consumption;

            EnergyConsumptionReport report = new EnergyConsumptionReport(DateTime.Now, consumption);
            energyConsumptionReports.Add(report);

            Console.WriteLine($"Zarejestrowany raport stanu energii: {report}");
        }
        else
        {
            Console.WriteLine("Automatyczne sterowanie oświetleniem jest wyłączone. Brak dostępnych danych dotyczących stanu energii.");
        }
    }

    // Metoda przeglądania raportów zużycia energii
    public void ViewEnergyConsumptionReports()
    {
        Console.WriteLine("Raporty stanu energii:");

        foreach (var report in energyConsumptionReports)
        {
            Console.WriteLine(report);
        }
    }


    // Zastępowanie metody w celu uzyskania dodatkowej funkcjonalności
    public override void MainFunction()
    {
        base.MainFunction();
        Console.WriteLine("Performing lighting and energy control function.");
    }
}

// Nowa klasa Zdalna kontrola budynku
class RemoteBuildingControl : SmartHome
{
    private bool remoteControlEnabled;

    // Ścieżka do pliku służącego do zapisywania informacji o pilocie
    private const string RemoteControlLogFile = "remote_control_log.txt";

    public RemoteBuildingControl(string homeName, AccessControl accessControl, FaultMessage faultMessage)
        : base(homeName, accessControl, faultMessage)
    {
        remoteControlEnabled = false;
    }

    // Funkcja umożliwiająca zdalne sterowanie
    public void TurnOnRemoteControl()
    {
        remoteControlEnabled = true;
        Console.WriteLine("Zdalna kontrola budynku jest teraz włączona.");
    }

    // Funkcja Zdalne sterowanie budynkiem
    public void TurnOffRemoteControl()
    {
        remoteControlEnabled = false;
        Console.WriteLine("Zdalna kontrola budynku jest teraz wyłączona.");
    }

    // Funkcja zdalnego sterowania
    public void RemoteControlFunction()
    {
        if (remoteControlEnabled)
        {
            Console.WriteLine("Wykonywanie czynności zdalnego sterowania.");

            // Do pliku zapisujemy informację o każdym wywołaniu funkcji
            string logEntry = $"{DateTime.Now}: Zdalne sterowanie zakończone.";
            File.AppendAllText(RemoteControlLogFile, logEntry + Environment.NewLine);
        }
        else
        {
            Console.WriteLine("Zdalna kontrola budynku nie jest włączona. Najpierw włącz tę opcję.");
        }
    }

    // Funkcja umożliwiająca przeglądanie historii logowań w trybie zdalnym
    public void ViewRemoteControlHistory()
    {
        // Odczytujemy informacje z pliku i wysyłamy je do konsoli
        string[] remoteControlHistory = File.ReadAllLines(RemoteControlLogFile);

        Console.WriteLine("Historia zdalnego sterowania:");

        if (remoteControlHistory.Length == 0)
        {
            Console.WriteLine("W historii zdalnego sterowania nie ma żadnych wpisów.");
        }
        else
        {
            foreach (var entry in remoteControlHistory)
            {
                Console.WriteLine(entry);
            }
        }
    }


    // Zastępowanie metody w celu uzyskania dodatkowej funkcjonalności
    public override void MainFunction()
    {
        base.MainFunction();
        Console.WriteLine("Performing remote building control function.");
    }
}

class Program
{
    static void Main()
    {
        AccessControl secureHome = new AccessControl("Secure Home", "1234");
        FaultMessage faultMessage = new FaultMessage("My Home Faults");
        SmartHome myHome = new SmartHome("My Home", secureHome, faultMessage);

        myHome.TurnOnSecuritySystem();
        myHome.MainFunction();

        // Uruchom menu główne
        myHome.MainMenu();
    }
}