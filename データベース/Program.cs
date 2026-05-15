//ここを顧客管理アプリのデータベース作成する場所にする
 
using Microsoft.Data.Sqlite;
namespace CustomerApp;
 
 class Program
{
    static void Main()
    {
        // 住所は一つ上の階層のデータベースフォルダの中
        var connectionString = "Data Source=/Users/misa/practice/顧客管理アプリ/データベース/account.db";
        //データベースへの接続
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        //テーブル作成（外枠を作るイメージ）
        var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
        -- 顧客管理テーブル
        CREATE TABLE IF NOT EXISTS customer_list (
            customer_id INTEGER PRIMARY KEY AUTOINCREMENT,
            customer_name TEXT(20) NOT NULL,
            customer_staffname TEXT(20) NOT NULL,
            customer_phone TEXT(15) NOT NULL,
            contact_staff TEXT(20) NOT NULL,
            deal TEXT(2) NOT NULL,
            email TEXT(50) NOT NULL,
            memo TEXT
        );

        -- 社員管理テーブル
        CREATE TABLE IF NOT EXISTS staff_list (
            staff_id INTEGER PRIMARY KEY AUTOINCREMENT,
            staff_name TEXT(20) NOT NULL,
            staff_sex TEXT(1) NOT NULL,
            staff_birth INTEGER(8) NOT NULL,
            staff_post TEXT(5) NOT NULL
        );";

        createCommand.ExecuteNonQuery();

    }
}