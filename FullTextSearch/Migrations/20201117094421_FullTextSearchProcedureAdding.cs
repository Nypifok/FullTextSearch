using Microsoft.EntityFrameworkCore.Migrations;

namespace FTSApp.Migrations
{
    public partial class FullTextSearchProcedureAdding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION setweight(query tsquery, weights text) RETURNS tsquery AS $$
                                    SELECT regexp_replace(
                                                query::text, 
                                                '(?<=[^ !])'':?(\*?)A?B?C?D?', ''':\1'||weights, 
                                                'g'
                                            )::tsquery;
                                    $$ LANGUAGE SQL IMMUTABLE;");
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION images_fts(search_query text, weights text,query_limit int) RETURNS 
                                    TABLE (images_id uuid,title text,key_words text,description text,image_vector tsvector,weight real) AS
                                    $$
                                        SELECT images_id,title,key_words,description, image_vector
                                    			  , image_vector <=> setweight(plainto_tsquery(search_query),weights) AS weight
                                    	FROM images
                                    	WHERE image_vector  @@ setweight(plainto_tsquery(search_query),weights)
                                    	ORDER BY weight ASC
                                    	LIMIT query_limit;
                                    $$ LANGUAGE SQL IMMUTABLE;");          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION images_fts;");
            migrationBuilder.Sql(@"DROP FUNCTION setweight;");
        }
    }
}
