# TheBookNook app

## Kort om appen.
![Skärmbild 2024-12-28 195906](https://github.com/user-attachments/assets/c3cd1193-0fd8-4795-adb5-743a54c632bd)
Det tog mig längre tid än förväntat att få ihop denna app och det beror på lite olika faktorer. Jag använde mig av DataBase First och min databas ställde till det en del för mig när det gäller att knyta den till koden bakom. Bidragande faktor till det är givetvis min begränsade kunskap. 

## Kända buggar
![Skärmbild 2024-12-28 200146](https://github.com/user-attachments/assets/e82d610c-8413-46fc-99a3-8e10577d7781)
Jag har satt vallidering på ISBN och titel. Den ser ut att fungera utmärkt för ISBN. Spara knappen aktiveras inte förrän rätt antal siffror matats in. När det gäller titeln så går det att gå förbi den om man bara hoppar över den. Har man däremot börjat skriva och sedan rader den texten så kickar valideringen in. Det finns heller ingen kontroll av prisfältet vilket innebär att appen kraschar om något annat än heltal eller ett decimal med komma tecken, t ex 25,99.

![Skärmbild 2024-12-28 200237](https://github.com/user-attachments/assets/7cc4f693-0dbb-4690-bfb6-21fb3fc8e997)
När det gäller att lägga till och ta bort böcker så får jag inte antal att uppdateras i UI. Det sker i bakgrunden och man ser ändringen om man byter till en annan butik och tillbaka igen. Jag har använt mig av OnPropertyChanged metoden på flertal olika egenskaper men hittar inte rätt.
