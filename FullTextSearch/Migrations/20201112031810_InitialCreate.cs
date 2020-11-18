using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

namespace FTSApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    images_id = table.Column<Guid>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    key_words = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    image_vector = table.Column<NpgsqlTsVector>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.images_id);
                });
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION make_tsvector(title TEXT, key_words TEXT,description TEXT)
                    RETURNS tsvector AS $$
                    BEGIN
                    RETURN (setweight(to_tsvector('english', title),'A') ||
                    setweight(to_tsvector('russian', title), 'A')||
                    setweight(to_tsvector('english', key_words), 'B')||
                    setweight(to_tsvector('russian', key_words), 'B')||
                    setweight(to_tsvector('english', description), 'C')||
                    setweight(to_tsvector('russian', description), 'C'));
                    END
                    $$ LANGUAGE 'plpgsql' IMMUTABLE;");
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION image_tsvector_trigger() RETURNS trigger AS $$
                    BEGIN
                      NEW.image_vector :=
                         make_tsvector(NEW.title,NEW.key_words,NEW.description);                        
                      RETURN NEW;
                    END
                    $$ LANGUAGE plpgsql;");

            migrationBuilder.Sql(@"CREATE EXTENSION rum;");

            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS image_vector_idx ON ""images"" 
                                    USING RUM (""image_vector"");");
            migrationBuilder.Sql(@"CREATE TRIGGER image_search_vector_update
                                    BEFORE INSERT OR UPDATE OF title,key_words,description ON ""images"" 
                                    FOR EACH ROW EXECUTE PROCEDURE image_tsvector_trigger();");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "images");
            migrationBuilder.Sql(@"DROP INDEX image_vector_idx;");
            migrationBuilder.Sql(@"DROP FUNCTION make_tsvector;");
            migrationBuilder.Sql(@"DROP EXTENSION rum;");
            migrationBuilder.Sql(@"DROP TRIGGER image_search_vector_update;");


        }
    }

}
