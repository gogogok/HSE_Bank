<h1>1) Общая идея решения (функционал) </h1>

<h2>Модуль «Учёт финансов»: </h2>

Счета: создание/переименование/удаление, хранение текущего баланса.
Категории: доход/расход, создание/переименование/смена типа/удаление.
Операции: добавление/удаление операций (доход/расход) с датой, суммой, описанием, категорией и привязкой к счёту.
Аналитика:
  доходы − расходыза период ,
  группировка по категориям для доходов/расходов.
Пересчёт балансов:
  Automatic - баланс обновляется сразу при Add/Delete,
  Manual - баланс пересчитывается по истории через пункт меню «Пересчитать баланс».
Импорт/экспорт: CSV / JSON / YAML.
Консольное меню: выбор действий, переключение режима пересчёта.
Слои проекта:
Domain/          сущности, enum, фабрика домена
Infrastructure/  хранилище, база данных, импорт/экспорт
Application/     фасады, стратегии пересчёта, команды
ConsoleApp/      DI-композиция, консольное меню

<h2>2) SOLID и GRASP — где реализовано</h2>

<h3>SOLID</h3>
S (Single Responsibility)
DomainFactory — только создание сущностей.
Facade — только бизнес-логикаI.
Csv/Json/YamlImporter — только парсинг/формирование текста.
InMemoryDb / CachedRepositoryProxy — хранение и кэширование соответственно.

O (Open/Closed)
Добавление нового формата импорта/экспорта или новой стратегии пересчёта НЕ требует менять существующие классы (через интерфейс).

L (Liskov Substitution)
IRecalcStrategy → AutomaticRecalcStrategy / ManualRecalcStrategy взаимозаменяемы.
AbstractImporter → Csv/Json/YamlImporter.
IExportVisitor → Csv/Json/YamlExportVisitor.

I (Interface Segregation)
Маленькие интерфейсы: IDomainFactory, IRecalcStrategy, IExportVisitor, IRepository.

D (Dependency Inversion)
Фасады зависят от абстракций (IRepository, IDomainFactory, IRecalcStrategy), внедрение через DI в ConsoleApp.


<h3>GRASP</h3>

High Cohesion / Low Coupling —  маленькие интерфейсы; формат импорта не «знает» доменную логику; фасад не знает деталей хранения.

Creator — DomainFactory создаёт BankAccount/Category/Operation.

Controller — Facade координирует сценарии.

Indirection — CachedRepositoryProxy как прослойка между приложением и хранилищем.

Polymorphism — стратегии/импортёры/визиторы выбираются и используются через интерфейсы.

<h2> 3) Использованные паттерны GoF (8 шт.) </h2>

Фабрика 
Класс: DomainFactory : IDomainFactory
Зачем: централизованное создание  BankAccount, Category, Operation
Где применяется: при импорте и при пользовательских сценариях Add/Create.

Стратегия 
Классы: IRecalcStrategy, AutomaticRecalcStrategy, ManualRecalcStrategy, RecalcStrategyContext
Зачем: выбор алгоритма пересчёта балансов (авто/ручной) в рантайме без if в фасаде.
Где применяется: OperationFacade делегирует OnAdd/OnDelete/Recompute текущей стратегии.

Шаблонный метод
Класс: AbstractImporter (шаблон Import() вызывает абстрактный Parse())
Зачем: общий алгоритм импорта (чтение файла, парсинг, создание сущностей), детализация парсинга в наследниках.
Где применяется: CsvImporter/JsonImporter/YamlImporter реализуют Parse().

Посетитель 
Классы: IExportVisitor, CsvExportVisitor, JsonExportVisitor, YamlExportVisitor
Зачем: разные форматы экспорта обходят одни и те же сущности, не меняя сами сущности.
Где применяется: пункт меню «Экспорт» - визитор накапливает результат и даёт расширение файла.

Фасад 
Классы: AccountFacade, CategoryFacade, OperationFacade, AnalyticsFacade
Зачем: инкапсуляция бд, фабрики, стратегий, команд.
Где применяется: консоль напрямую общается только с фасадами.

Команда 
Классы: CreateAccountCmd, AddOperationCmd (+ TimedCommandDecorator)
Зачем: завернуть пользовательские действия в объекты (удобнее работать через клманды)
Где применяется: пункты меню «Создать счёт», «Добавить операцию».

Декоратор 
Класс: TimedCommandDecorator
Зачем: добавление измерения времени к любой команде без изменения её кода.
Где применяется: обёртка вокруг Execute().

Прокси 
Классы: CachedRepositoryProxy : IRepository
Зачем: кэширование и перехват вызовов репозитория, ускорение Find/All, изоляция от реального источника данных.
Где применяется: регистрируется вместо «сырых» источников

<h1>Привязка функционала к паттернам (кратко)</h1>

Авто/ручной пересчёт баланса: Стратегия + Фасад (OperationFacade, IRecalcStrategy).

Импорт: Шаблонный метод (AbstractImporter) + Фабрика (создание сущностей через фабрику).

Экспорт: Посетитель (разные форматы без изменения сущностей).

Меню: Команда (+ Декоратор для тайминга), вызовы идут через Фасад.

Производительность чтения: Прокси (CachedRepositoryProxy).
