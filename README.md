# SBES_BankSystem

Projekat na 4. godini studija Fakulteta Tehničkih nauka u Novom Sadu.<br>
Predmet: Sigurnost i bezbednost elektroenergetskih sistema.

Cilj projekta je implementirati integrisani sistem plaćanja usluga mobilnih operatera koristeći SOA Gateway mehanizam i WCF framework.<br>
Infrastruktura sistema se sastoji od Bankarskog servisa, N broja operatera, i N broja klijenata.

SOA Gateway mehanizam služi za rutiranje saobraćaja između komponenti i zaštitu od neželjenih napada.

Takođe rešenje zahteva da se poruke koje se razmenjuju između komponenti kriptuju Triple DES (CBC i ECB) algoritmima, uz upotrebu sertifikata.

Svi događaji su zapisani u Windows Event Log-u.
