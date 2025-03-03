# CompanyManager With Generic

**Lernziele:**

- Wie eine generische Klasse `EntitySet<T>` zum Anpassen von `Create`, `Insert`, `Update` und `Delete` Operationen erstellt wird.

**Hinweis:** Als Startpunkt wird die Vorlage [CompanyManagerWithSqlite](https://github.com/leoggehrer/CompanyManagerWithSqlite) verwendet.

## Vorbereitung

Bevor mit der Umsetzung begonnen wird, sollte die Vorlage heruntergeladen und die Funktionalität verstanden werden.

### Analyse der `DbSet<T>`-Eigenschaft in der Klasse `CompanyContext`

Für jede Entität (`Company`, `Customer` und `Employee`) gibt es im Context (`CompanyContext`) eine Eigenschaft vom Typ `DbSet<T>`. Das bedeutet, dass der Zugriff für die CRUD-Operationen der entsprechende Entität über diese Eigenschaft erfolgt. Allerdings hat der Context keine Kontrolle, wenn diese Operationen direkt am `DbSet<T>` erfolgen. In vielen Anwendungsfällen ist es jedoch erforderlich, dass zusätzliche Aktionen bei den Standard-Operationen (CRUD) ausgeführt werden. Dies führt dazu, dass wir eine generische Klasse `EntitySe<T>` entwickeln und den Zugriff auf diese Klasse bzw. deren Ableitung umleiten.

#### Implementierung der generischen Klasse `EntitySet<T>`

Wenn die Standard-Operationen (CRUD) angepasst werden sollen, dann müssen die CRUD-Operationen vom `DbSet<T>` verdeckt und in die generische Klasse ausgelaget werden. Dies geschieht dadurch, dass das `DbSet<T>` in der generischen Klasse eingebettet ist und die entsprechenden Operation an das `DbSet<T>` weiter geleitet werden. Damit kann ein Steuercode, durch das Überschreiben von Methoden, zwischen den Operationen geschaltet werden.

Im nachfolgenden Abschnitt befindet sich die generische Klasse `EntitySet<TEntity>`:

```csharp
namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Represents a set of entities that can be queried from a database and provides methods to manipulate them.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class EntitySet<TEntity> where TEntity : Entities.EntityObject, new()
    {
        #region fields
        protected DbSet<TEntity> _dbSet;
        #endregion fields

        #region properties
        /// <summary>
        /// Gets the database context.
        /// </summary>
        internal DbContext Context { get; private set; }

        /// <summary>
        /// Gets the queryable set of entities.
        /// </summary>
        public IQueryable<TEntity> QuerySet => _dbSet.AsQueryable();
        #endregion properties

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySet{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="dbSet">The set of entities.</param>
        public EntitySet(DbContext context, DbSet<TEntity> dbSet)
        {
            Context = context;
            _dbSet = dbSet;
        }

        #region methods
        protected abstract void CopyProperties(TEntity target, TEntity source);

        /// <summary>
        /// Creates a new instance of the entity.
        /// </summary>
        /// <returns>A new instance of the entity.</returns>
        public virtual TEntity Create()
        {
            return new TEntity();
        }

        /// <summary>
        /// Adds the specified entity to the set.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        public virtual TEntity Add(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        /// <summary>
        /// Asynchronously adds the specified entity to the set.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added entity.</returns>
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _dbSet.AddAsync(entity).ConfigureAwait(false);

            return result.Entity;
        }

        /// <summary>
        /// Updates the specified entity in the set.
        /// </summary>
        /// <param name="id">The identifier of the entity to update.</param>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>The updated entity, or null if the entity was not found.</returns>
        public virtual TEntity? Update(int id, TEntity entity)
        {
            var existingEntity = _dbSet.Find(id);
            if (existingEntity != null)
            {
                CopyProperties(existingEntity, entity);
            }
            return existingEntity;
        }

        /// <summary>
        /// Asynchronously updates the specified entity in the set.
        /// </summary>
        /// <param name="id">The identifier of the entity to update.</param>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated entity, or null if the entity was not found.</returns>
        public virtual async Task<TEntity?> UpdateAsync(int id, TEntity entity)
        {
            var existingEntity = await _dbSet.FindAsync(id).ConfigureAwait(false);
            if (existingEntity != null)
            {
                CopyProperties(existingEntity, entity);
            }
            return existingEntity;
        }

        /// <summary>
        /// Removes the entity with the specified identifier from the set.
        /// </summary>
        /// <param name="id">The identifier of the entity to remove.</param>
        /// <returns>The removed entity, or null if the entity was not found.</returns>
        public virtual TEntity? Remove(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            return entity;
        }
        #endregion methods
    }
}
```

Die Klasse `EntityStet<T>` kabselt den Zugriff auf die Eigenschaft von `DbSet<T>` und bietet für die Standard-Operationen (CRUD) eigene Methode an. Diese Methoden sind alle mit dem Spezifizierer `virtual` markiert und können bei Bedarf in der Ableitung überschrieben werden. Im nachfolgenden Abschnitt befinden sich die konkretisierten Klassen für die einzelnen Entitäten:

```csharp
namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Represents a set of <see cref="Entities.Company"/> entities in the data context.
    /// </summary>
    internal sealed class CompanySet : EntitySet<Entities.Company>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanySet"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="dbSet">The database set of <see cref="Entities.Company"/> entities.</param>
        public CompanySet(DbContext context, DbSet<Entities.Company> dbSet) : base(context, dbSet)
        {
        }

        /// <summary>
        /// Copies the properties from the source <see cref="Entities.Company"/> to the target <see cref="Entities.Company"/>.
        /// </summary>
        /// <param name="target">The target <see cref="Entities.Company"/>.</param>
        /// <param name="source">The source <see cref="Entities.Company"/>.</param>
        protected override void CopyProperties(Entities.Company target, Entities.Company source)
        {
            (target as Common.Contracts.ICompany).CopyProperties(source);
        }
    }
}

namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Represents a set of customer entities in the data context.
    /// </summary>
    internal sealed class CustomerSet : EntitySet<Entities.Customer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerSet"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="dbSet">The DbSet of customer entities.</param>
        public CustomerSet(DbContext context, DbSet<Entities.Customer> dbSet) : base(context, dbSet)
        {
        }

        /// <summary>
        /// Copies the properties from the source customer entity to the target customer entity.
        /// </summary>
        /// <param name="target">The target customer entity.</param>
        /// <param name="source">The source customer entity.</param>
        protected override void CopyProperties(Entities.Customer target, Entities.Customer source)
        {
            (target as Common.Contracts.ICustomer).CopyProperties(source);
        }
    }
}

namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Represents a set of Employee entities in the data context.
    /// </summary>
    internal sealed class EmployeeSet : EntitySet<Entities.Employee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeSet"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="dbSet">The DbSet of Employee entities.</param>
        public EmployeeSet(DbContext context, DbSet<Entities.Employee> dbSet) : base(context, dbSet)
        {
        }

        /// <summary>
        /// Copies the properties from the source Employee entity to the target Employee entity.
        /// </summary>
        /// <param name="target">The target Employee entity.</param>
        /// <param name="source">The source Employee entity.</param>
        protected override void CopyProperties(Entities.Employee target, Entities.Employee source)
        {
            (target as Common.Contracts.IEmployee).CopyProperties(source);
        }
    }
}
```

Die Ableitungen sind einfach gehalten. Es muss der Konstruktor und die entsprechende Methode `CopyProperties(...)` implementiert werden. Der Konstruktor leitet die Parameter `DbContext` und den `DbSte<T>` an die Oberklasse weiter. Die Methode `CopyProperties(...)` kann nur in den konkretisierten Klassen implementiert werden. Denn nur auf dieser Ebene ist die `CopyProperties(...)` der entsprechenden Schnittstelle bekannt. Zur Erinnerung: In den Schnittstellen ist die Methode `CopyProperties(...)` als Default-Implementierung definiert.

### Anpassung der Klasse `CompanyContext` und der Schnittstelle `IContext`

Jetzt muss die Klasse `CompanyContext` und die entsprechende Schnittstelle `IContext` angepasst werden. Die Klasse `CompanyContext` darf keinen öffentlichen Zugriff auf die entsprechenden `DbSet<T>` ermöglichen. Daher müssen diese Eigenschaften auf `internal` gesetzt werden. Anstatt desen werden die Eigenschaften `EntitySet<T>` öffentlich gesetzt. Nun müssen alle Operation über das `EntitySet<T>` erfolgen und können somit angepasst werden.

Nachfogend die angepasste Klasse `CompanyContext`:

```csharp
namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Represents the database context for the CompanyManager application.
    /// </summary>
    internal class CompanyContext : DbContext, IContext
    {
        #region fields
        /// <summary>
        /// The type of the database (e.g., "Sqlite", "SqlServer").
        /// </summary>
        private static string DatabaseType = "Sqlite";

        /// <summary>
        /// The connection string for the database.
        /// </summary>
        private static string ConnectionString = "data source=CompanyManager.db";
        #endregion fields

        /// <summary>
        /// Initializes static members of the <see cref="CompanyContext"/> class.
        /// </summary>
        static CompanyContext()
        {
            var appSettings = Common.Modules.Configuration.AppSettings.Instance;

            DatabaseType = appSettings["Database:Type"] ?? DatabaseType;
            ConnectionString = appSettings[$"ConnectionStrings:{DatabaseType}ConnectionString"] ?? ConnectionString;
        }

        #region properties
        /// <summary>
        /// Gets or sets the DbSet for Company entities.
        /// </summary>
        internal DbSet<Entities.Company> DbSetCompany { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Customer entities.
        /// </summary>
        internal DbSet<Entities.Customer> DbSetCustomer { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Employee entities.
        /// </summary>
        internal DbSet<Entities.Employee> DbSetEmployee { get; set; }

        /// <summary>
        /// Gets the EntitySet for Company entities.
        /// </summary>
        public EntitySet<Entities.Company> CompanySet { get; }

        /// <summary>
        /// Gets the EntitySet for Customer entities.
        /// </summary>
        public EntitySet<Entities.Customer> CustomerSet { get; }

        /// <summary>
        /// Gets the EntitySet for Employee entities.
        /// </summary>
        public EntitySet<Entities.Employee> EmployeeSet { get; }
        #endregion properties

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyContext"/> class.
        /// </summary>
        public CompanyContext()
        {
            CompanySet = new CompanySet(this, DbSetCompany!);
            CustomerSet = new CustomerSet(this, DbSetCustomer!);
            EmployeeSet = new EmployeeSet(this, DbSetEmployee!);
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Configures the database context options.
        /// </summary>
        /// <param name="optionsBuilder">The options builder to be used for configuration.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (DatabaseType == "Sqlite")
            {
                optionsBuilder.UseSqlite(ConnectionString);
            }
            else if (DatabaseType == "SqlServer")
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }
        #endregion methods
    }
}
```

Natürlich muss die Schnittstelle `IContext` an die Änderung angepasst werden:

```csharp
namespace CompanyManager.Logic.Contracts
{
    /// <summary>
    /// Represents a context that provides access to entity sets and supports saving changes.
    /// </summary>
    public interface IContext : IDisposable
    {
        /// <summary>
        /// Gets the entity set for companies.
        /// </summary>
        EntitySet<Entities.Company> CompanySet { get; }

        /// <summary>
        /// Gets the entity set for customers.
        /// </summary>
        EntitySet<Entities.Customer> CustomerSet { get; }

        /// <summary>
        /// Gets the entity set for employees.
        /// </summary>
        EntitySet<Entities.Employee> EmployeeSet { get; }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of state entries written to the underlying database.</returns>
        int SaveChanges();
    }
}
```

### Testen des Systems

- Testen Sie Anwendung mit dem Konsolenprogramm.

## Hilfsmittel

- keine

## Abgabe

- Termin: 1 Woche nach der Ausgabe
- Klasse:
- Name:

## Quellen

- keine Angabe

> **Viel Erfolg!**
