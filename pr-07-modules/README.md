# ПР №7. Модульная структура проекта

Вариант 3: АИС «Электронный магазин / Маркетплейс кампуса».

- `Практическая_7_Браунков_02.02.pdf` — отчёт: модули клиента, диаграмма компонентов UML, модель C4, дерево решения Visual Studio.
- `CampusMarket/CampusMarket.sln` — решение Visual Studio из четырёх проектов, как в отчёте (рисунок 4).

| Проект | Папки и ключевые классы | Ссылается на |
|---|---|---|
| `CampusMarket.UI.Desktop` (WPF) | `Views` (`CatalogView`, `CartView`), `ViewModels` (`CatalogViewModel`, `RelayCommand`), `App` | Core.BusinessLogic, Security.Auth |
| `CampusMarket.Core.BusinessLogic` | `Services` (`CartService`, `CatalogService`, `OrderService`), `Abstractions` (`ICatalog`, `IOrder`) | Data.ApiClient, Security.Auth |
| `CampusMarket.Data.ApiClient` | `Models` (`CatalogItem`, `Checkout`), `IApiClient`, `CampusApiClient` | Security.Auth |
| `CampusMarket.Security.Auth` | `AuthService`, `TokenStore`, `ITokenProvider` | — |

Ссылки идут только в одну сторону, циклов нет. Интерфейс `IApiClient` лежит в `Data.ApiClient`, а не в `Core.BusinessLogic/Abstractions`: иначе бизнес-логика и сетевой слой ссылались бы друг на друга.

Открыть решение: Visual Studio 2022 на Windows, .NET 8 и рабочая нагрузка «Разработка классических приложений .NET». Адрес шлюза задаётся переменной `CAMPUS_API_URL` (по умолчанию `http://localhost:5080/api/v1/`).
