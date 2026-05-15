using Microsoft.Data.Sqlite;
namespace 顧客管理アプリ実行コード;

class Program
{
    static void Main ()
    {
        // ① まず、道具（connection）をここで作る
        //connection=DBと繋がっている電話線
        var connectionString = "Data Source=/Users/misa/practice/顧客管理アプリ/データベース/account.db";
        using var connection = new SqliteConnection(connectionString);
        connection.Open(); // ここで「connection」という名前がつく

        //1回で終わらないようにループ処理
        while (true)
        {
            //メニュー画面表示
            Console.WriteLine("-----顧客管理システム-----");
            Console.WriteLine("[1] 顧客検索");
            Console.WriteLine("[2] 顧客登録");
            Console.WriteLine("[3] 社員検索");
            Console.WriteLine("[4] 社員登録");
            Console.WriteLine("[q] アプリ終了");
            Console.WriteLine("番号を選択してください");

            //入力された数字を文字列として読み込む
            string menu = Console.ReadLine();

            //1なら「顧客検索画面」に遷移
            if(menu == "1")
            {
                while(true)
                {
                    Console.Clear(); //ループの最初にコンソールを綺麗にする
                    Console.WriteLine("顧客検索");

                    //最初に入力項目一覧表示
                    Console.WriteLine(@"
                    顧客名 :
                    顧客担当者名 :
                    電話番号 :
                    メールアドレス :
                    自社担当者名 :
                    取引有無
                    ◻︎ 有  ◻︎ 無  ◻︎すべて
                    [t] 閉じる
                    [r] 新規登録
                    ");

                    //検索画面作る（入力を受け付ける）
                    Console.Write("顧客名: ");
                    string searchName = Console.ReadLine() ?? "";
                     if(searchName.ToLower() == "t") break;
                     if(searchName.ToLower() == "r") 
                     {
                        Console.WriteLine("顧客登録画面に遷移します");
                        menu = "2"; break;
                     }

                    Console.Write("顧客担当者名: ");
                    string searchcustStaff = Console.ReadLine() ?? "";
                     if(searchcustStaff.ToLower() == "t") break;
                     if(searchcustStaff.ToLower() == "r") 
                     {
                        Console.WriteLine("顧客登録画面に遷移します");
                        menu = "2"; break;
                     }

                    Console.Write("電話番号: ");
                    string searchPhone = Console.ReadLine() ?? "";
                     if(searchPhone.ToLower() == "t") break;
                     if(searchPhone.ToLower() == "r") 
                     {
                        Console.WriteLine("顧客登録画面に遷移します");
                        menu = "2"; break;
                     }

                    Console.Write("メールアドレス: ");
                    string searchemail = Console.ReadLine() ?? "";
                     if(searchemail.ToLower() == "t") break;
                     if(searchemail.ToLower() == "r") 
                     {
                        Console.WriteLine("顧客登録画面に遷移します");
                        menu = "2"; break;
                     }

                    Console.Write("自社担当者名: ");
                    string searchmyStaff = Console.ReadLine() ?? "";
                     if(searchmyStaff.ToLower() == "t") break;
                     if(searchmyStaff.ToLower() == "r") 
                     {
                        Console.WriteLine("顧客登録画面に遷移します");
                        menu = "2"; break;
                     }

                    Console.Write("取引有無: ");
                    string searchdeal = Console.ReadLine() ?? "";
                     if(searchdeal.ToLower() == "t") break;
                     if(searchdeal.ToLower() == "r") 
                     {
                        Console.WriteLine("顧客登録画面に遷移します");
                        menu = "2"; break;
                     }

                    //項目入力後、ボタン選択
                    Console.WriteLine("\n------------------------------");
                    Console.WriteLine("[s] 検索  [r] 新規登録  [t] 閉じる");
                    Console.Write("アクションを選択してください: ");
                    string action = Console.ReadLine() ?? "";

                    // DBに「探しに行く」命令を送る
                    if(action == "s")
                    {
                        SearchCustomers(connection, searchName, searchcustStaff, searchPhone, searchmyStaff, searchdeal, searchemail);

                        Console.WriteLine("\nEnterを押すと検索画面に戻ります。");
                        Console.ReadLine();
                    }
                    if(action == "t") continue;
                    if(action == "r")
                    {
                        Console.WriteLine("顧客登録画面に遷移します");
//★★★顧客登録画面に遷移する
                    }

                }

            }

            //2なら「顧客登録画面」に遷移
            else if(menu == "2")
            {
                while(true)
                {
                    Console.WriteLine("顧客登録");
                    //最初に入力項目一覧表示
                    Console.WriteLine(@"
                    顧客ID :
                    顧客名 :
                    顧客担当者名 :
                    電話番号 :
                    メールアドレス :
                    自社担当者名 :
                    取引有無
                    ◻︎ 有  ◻︎ 無
                    備考 :
                    [t] 閉じる
                    ");

                    // 部品を呼び出す（引数でルールを伝える）
                    //もし「t」を選択していれば、メニュー画面に戻る
                    string name      = Check("顧客名", 20, true);
                    if(name == "CANCEL_SIGNAL") break;

                    string custStaff = Check("顧客担当者名", 20, true);
                    if(custStaff == "CANCEL_SIGNAL") break;

                    string phone     = Check("電話番号", 15, true);
                    if(phone == "CANCEL_SIGNAL") break;

                    string mail      = Check("メールアドレス", 50, true);
                    if(mail == "CANCEL_SIGNAL") break;

                    string myStaff   = Check("自社担当者名", 20, true);
                    if(myStaff == "CANCEL_SIGNAL") break;

                    string deal      = Check("取引有無 (有/無)", 2, true);
                    if(deal == "CANCEL_SIGNAL") break;

                    string memo   = Check("備考", 100, false); // 備考は必須じゃなくてOK
                    if(memo == "CANCEL_SIGNAL") break;


                    // DBへの保存処理（ここは今までと同じ）
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = @"
                        INSERT INTO customer_list (customer_name, customer_staffname, customer_phone, contact_staff, deal, email, memo) 
                        VALUES ($n, $c, $p, $ms, $d, $m, $me)";
            
                    cmd.Parameters.AddWithValue("$n", name);
                    cmd.Parameters.AddWithValue("$c", custStaff);
                    cmd.Parameters.AddWithValue("$p", phone);                        
                    cmd.Parameters.AddWithValue("$ms", myStaff);
                    cmd.Parameters.AddWithValue("$d", deal);
                    cmd.Parameters.AddWithValue("$m", mail);
                    cmd.Parameters.AddWithValue("$me", memo);
            
                    //項目入力後、ボタン選択
                    Console.WriteLine("\n------------------------------");
                    Console.WriteLine("[r] 登録/更新  [c] クリア  [t] 閉じる");
                    Console.Write("アクションを選択してください: ");
                    string action = Console.ReadLine() ?? "";

                    if(action == "r")
                    {
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("\n登録が完了しました。");
                        Console.ReadLine();
                    }
                    else if(action == "c")
                    {
                       continue;
                    }                        
                    else if(action == "t")
                    {
                        Console.WriteLine("メニュー画面に戻ります");
                        break;
                    }
                }                
            }
        




            //3なら「社員検索画面」に遷移
            else if(menu == "3")
            {
                Console.WriteLine("社員検索");
            }

            //4なら「社員登録画面」に遷移
            else if(menu == "4")
            {
                Console.WriteLine("社員登録");
            }
            else if(menu == "q")
            {
                Console.WriteLine("アプリを終了します。");
                return;
            }
            else
            {
                Console.WriteLine(@"
                エラーが発生しました。
                メニューに戻ります。");
            }
        }
    }

    //---------------------------------------------------------------------------------------------
    //入力ミス判定のCheckメソッド
    // label: 表示名, maxLength: 最大文字数, isRequired: 必須かどうか
    static string Check(string label, int maxLength, bool isRequired)
    {
        while (true) // 合格するまで無限ループ
        {
            Console.Write($"{label}: ");
            string input = Console.ReadLine() ?? "";

            // もし t が入力されたら、特別な「合図」を Main に返す
            if (input.ToLower() == "t")
            {
                return "CANCEL_SIGNAL";
            }

            // 必須チェック
            if (isRequired && string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine($"   [!] エラー：{label}は必須入力です。");
                continue; // whileの先頭に戻って再入力
            }
        
            // 文字数チェック
            if (input.Length > maxLength)
            {
                Console.WriteLine($"   [!] エラー：{label}は{maxLength}文字以内で入力してください。");
                continue; // whileの先頭に戻って再入力
            }

            return input; // 合格！ループを抜けて、入力された文字を呼び出し元に返す
        }
    }
//-------------------------------------------------------------------------------------------------
    //今からSearchCustomersという部屋を作る
    //name: ユーザーが入力した「探したい顧客名」
    //phone: ユーザーが入力した「探したい電話番号」
    static void SearchCustomers(SqliteConnection connection, string name, string custStaff, string phone, string myStaff, string deal, string email)
    {
        //空の注文書(cmd)を作成
        var cmd = connection.CreateCommand();

        //注文内容を書く
        //customer_listから
        //顧客名がnに似てて、電話番号がpに似ている人
        cmd.CommandText = @"
        SELECT customer_id, customer_name, customer_staffname, customer_phone, contact_staff, deal, email 
        FROM customer_list
        WHERE customer_name LIKE $n 
            AND customer_staffname LIKE $csn
            AND customer_phone LIKE $p 
            AND contact_staff LIKE $cs
            AND deal LIKE $d
            AND email LIKE $e";

        //$nと$pに検索ワードを当てはめる
        //文字を%で挟むと、その文字を含んでいればなんでもOKという意味になる（部分一致）
        cmd.Parameters.AddWithValue("$n", "%" + name + "%");
        cmd.Parameters.AddWithValue("$csn", "%" + custStaff + "%");
        cmd.Parameters.AddWithValue("$p", "%" + phone + "%");
        cmd.Parameters.AddWithValue("$cs", "%" + myStaff + "%");
        cmd.Parameters.AddWithValue("$d", "%" + deal + "%");
        cmd.Parameters.AddWithValue("$e", "%" + email + "%");


        //実行する
        // using は読み取りが終わったら、自動的にスキャナーの片付け（メモリの解放）をしてくれる
        using var reader = cmd.ExecuteReader();

        //表の見出しを作る
        Console.WriteLine("\n【検索結果】");
        Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        Console.WriteLine($"{"ID",-5} | {"顧客名",-20} | {"担当者名",-20} | {"電話番号",-15} | {"自社担当者名",-20} | {"取引",-2} | {"メールアドレス",-50}");
        Console.WriteLine("---------------------------------------------------------------------------------------------------------");

        //データを順番に取り出して表に合わせて並べる
        // {reader[0],-5}とは、一番最初に取り出したデータを左詰めでおいて、5マス分の幅が確保されていること
        while (reader.Read())
        {
            Console.WriteLine($"{reader[0],-5} | {reader[1],-20} | {reader[2],-20} | {reader[3],-15} | {reader[4],-20} | {reader[5],-2} | {reader[6],-50}");
        }

        Console.WriteLine("---------------------------------------------------------------------------------------------------------");
    
        ShowSubMenu();

    }   

//ShowSubMenuの部屋を作る--------------------------------------------------------------------------------
    static void ShowSubMenu()
    {
        Console.WriteLine("[e] 編集  [d] 削除");
        Console.Write("アクションを選択してください: ");
        string subAction = Console.ReadLine() ?? "";

//eの場合、選択しているデータを顧客登録画面で開く
        if(subAction == "e")
        {
            Console.WriteLine("★★★顧客登録画面に遷移する処理");
        }


//dの場合、データ削除画面に移行
        if(subAction == "d")
        {
            Console.WriteLine("★★★データ削除画面に遷移する処理");
        }


    }
}
