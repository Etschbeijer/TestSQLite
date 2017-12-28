
#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\netstandard.dll"
#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.dll"
#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.Sqlite.dll"

open Microsoft.EntityFrameworkCore

///Defining the Types/////////////////////////
//////////////////////////////////////////////////////////////////

type Blog(blogid : int, url : string) =
    let mutable blogID = blogid
    let mutable url    = url
    member this.BlogID with get() = blogID and set(value) = blogID <- value
    member this.URL    with get() = url    and set(value) = url    <- value

type Post(postid : int, title : string, content : string, blogid : int) =
    let mutable postID  = postid
    let mutable title   = title
    let mutable content = content
    let mutable blogID  = blogid
    member this.PostID  with get() = postID  and set(value) = postID  <- value
    member this.Title   with get() = title   and set(value) = title   <- value
    member this.Content with get() = content and set(value) = content <- value
    member this.BlogID  with get() = blogID  and set(value) = blogID  <- value


type BloggingContext() =
    inherit DbContext()

    [<DefaultValue>] val mutable m_blog : DbSet<Blog>
    member public this.Blog with get() = this.m_blog 
                                        and set v = this.m_blog  <- v

    [<DefaultValue>] val mutable m_post : DbSet<Post>
    member public this.Post with get() = this.m_post 
                                        and set v = this.m_post <- v

    override this.OnConfiguring (optionsbuilder :  DbContextOptionsBuilder) =
        optionsbuilder.UseSqlite(@"Data Source=C:\F#-Projects\blogging.db") |> ignore

type Abteilungen(abteilungenid : int, name : string) =
    let mutable abteilungenID = abteilungenid
    let mutable name     = name
    member this.AbteilungenID with get() = abteilungenID and set(value) = abteilungenID <- value
    member this.Name          with get() = name          and set(value) = name          <- value

type Rollen(rollenid : int, name : string) =
    let mutable rollenID = rollenid
    let mutable name     = name
    member this.RollenID with get() = rollenID and set(value) = rollenID <- value
    member this.Name     with get() = name     and set(value) = name     <- value

type PersonenVerzeichnis(id : int, name : string, abteilungsid : int, rollenid : int) =
    let mutable id           = id
    let mutable name         = name
    let mutable abteilungsID = abteilungsid
    let mutable rollenID     = rollenid
    member this.ID           with get() = id           and set(value) = id           <- value
    member this.Name         with get() = name         and set(value) = name         <- value
    member this.AbteilungsID with get() = abteilungsID and set(value) = abteilungsID <- value
    member this.RollenID     with get() = rollenID     and set(value) = rollenID     <- value

type PersonenContext() =
    inherit DbContext()
    
    [<DefaultValue>] val mutable m_abteilungenid : DbSet<Abteilungen>
    member public this.Abteilungen with get() = this.m_abteilungenid
                                           and set value = this.m_abteilungenid <- value


    [<DefaultValue>] val mutable m_rollen : DbSet<Rollen>
    member public this.Rollen with get() = this.m_rollen
                                           and set value = this.m_rollen <- value

    [<DefaultValue>] val mutable m_personenverzeichnis : DbSet<PersonenVerzeichnis>
    member public this.PersonenVerzeichnis with get() = this.m_personenverzeichnis
                                                        and set value = this.m_personenverzeichnis <- value

    override this.OnConfiguring (optionsbuilder :  DbContextOptionsBuilder) =
        optionsbuilder.UseSqlite(@"C:\F#-Projects\TestDatenBank.db") |> ignore


///Manipulating the databases/////////////////////////
/////////////////////////////////////////////////////////////////////

let program (id : int) (url : string) =
    let db = new BloggingContext()
    db.Add(new Blog(id, url)) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count

let program2 (id : int) (title : string) (content : string) (blogid : int) =
    let db = new BloggingContext()
    db.Add(new Post(id, title, content, blogid)) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count

let sqlTestingAbteilungen (id : int) (name : string) =
    let db = new PersonenContext()
    db.Add(new Abteilungen(id, name)) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count

let sqlTestingRollen (id : int) (name : string) =
    let db = new PersonenContext()
    db.Add(new Rollen(id, name)) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count

let sqlTestingPersonenVerzeichnis (id : int) (name : string) =
    let db = new PersonenContext()
    db.Add(new PersonenVerzeichnis(id, name, 1, 1)) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count


program 2 "http://sample.com"
program2 2 "jo" "joo" 1

sqlTestingAbteilungen 1 "BioTech"
sqlTestingRollen 1 "Professor"
sqlTestingPersonenVerzeichnis 1 "Bob"