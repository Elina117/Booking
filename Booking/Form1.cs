using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Booking
{
    
    public partial class Form1 : Form
    {
        private User user;
        private Book book;

        private void Init()
        {
           
            user = new User();
            book = new Book();
           
        }
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=ep-lucky-rice-772590.us-east-2.aws.neon.tech;Port=5432;User Id=elina3galimova;Password=6mSLTHBxAth1;Database=booking");
        }
        private void button_enter_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button_enter_end_Click(object sender, EventArgs e)
        {
            if(textBox_email_enter.Text == string.Empty && textBox_password_enter.Text == string.Empty)
            {
                textBox_email_enter.BackColor = Color.Red;
                textBox_password_enter.BackColor = Color.Red;
                MessageBox.Show("Неверно заполнены поля!");
                return;
            }
           
            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();
           
            cmd.CommandText = $"SELECT \"id\" FROM \"registration\" WHERE email = '{textBox_email_enter.Text}' AND password_ = '{textBox_password_enter.Text}'";
         
            try
            {
                conn.Open();
            

                if (conn.State == ConnectionState.Open)
                {
                 
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                
                    if (dr.Read())
                    {
                   
                        textBox_email_enter.Text = string.Empty;
                        textBox_password_enter.Text = string.Empty;
                    
                        user.id = dr.GetInt32(0);
                        PersonalSpace();
                      

                        tabControl1.SelectedIndex = 3;
                    }
                    else
                    {
                        textBox_email_enter.BackColor = Color.Red;
                        textBox_email_enter.BackColor = Color.Red;
                        MessageBox.Show("Неправильный логин или пароль!");
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            finally
            {
                conn.Close();

            }
        }

        public void PersonalSpace()
        {
            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM \"registration\" WHERE \"id\" = {user.id}";

            try
            {
                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        user.id = dr.GetInt32(0);
                        user.name = dr.GetString(1);
                        user.surname = dr.GetString(2);
                        user.patronymic = dr.GetString(3);
                        user.email = dr.GetString(4);
                        user.password = dr.GetString(5);



                        dr.Close();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

            private void button_registration_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button_reg_create_end_Click(object sender, EventArgs e)
        {
            if (textBox_reg_email.Text == string.Empty | textBox_reg_familia.Text == string.Empty | textBox_reg_grfath.Text == string.Empty |
                textBox_reg_name.Text == string.Empty | textBox_reg_password.Text == string.Empty | textBox_reg_password2.Text == string.Empty |
                checkBox_reg_age18.Checked == false | checkBox_reg_agreement.Checked == false)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            if (textBox_reg_password.Text != textBox_reg_password2.Text)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();
            NpgsqlDataReader reader;

            try
            {

                conn.Open();

                if (conn.State == ConnectionState.Open)
                {

                    cmd.CommandText = $"SELECT EXISTS (SELECT * from \"registration\" WHERE email = '{textBox_reg_email.Text}')";
                    reader = cmd.ExecuteReader();

                    if (reader.Read() && reader.GetBoolean(0) == true)
                    {
                        MessageBox.Show("Данная почта уже занята!");
                        return;
                    }
                    reader.Close();

                    cmd.CommandText = $"INSERT INTO \"registration\"(surname, name_, patronymic, email, password_) values ('{textBox_reg_familia.Text}', '{textBox_reg_name.Text}', '{textBox_reg_grfath.Text}', '{textBox_reg_email.Text}', '{textBox_reg_password.Text}')";
                    reader = cmd.ExecuteReader();


                    MessageBox.Show($"Форма заполнена верно");

                    tabControl1.SelectedIndex = 3;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void button_reg_file_agreement_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Соглашение на обработку персональных данных\r\nНастоящее соглашение регулирует порядок обработки персональных данных пользователей приложения бронирования гостиницы.\r\nЦели обработки персональных данных: Персональные данные предоставляются для осуществления бронирования номеров в гостинице, предоставления информации о бронировании, а также для обратной связи с клиентами.\r\nСостав персональных данных: К персональным данным могут относиться имя, фамилия, контактный номер телефона, адрес электронной почты, данные паспорта (при необходимости), иная информация, необходимая для оформления бронирования.\r\nСпособы обработки данных: Персональные данные обрабатываются с использованием средств автоматизации и без использования таковых, в рамках текущего приложения бронирования и для целей, указанных в пункте 1.\r\nСогласие на обработку данных: Пользователь, регистрируясь в приложении и размещая бронь, дает согласие на обработку своих персональных данных в соответствии с условиями настоящего соглашения.\r\nСрок хранения данных: Персональные данные хранятся до момента завершения оформленного бронирования и могут храниться дополнительно в соответствии с требованиями законодательства.\r\nЗащита данных: Администратор обязуется предпринимать все необходимые меры для защиты персональных данных от неправомерного доступа, изменения, распространения или уничтожения.\r\nПередача данных третьим лицам: Персональные данные могут передаваться третьим лицам только в случаях, предусмотренных действующим законодательством, либо с согласия субъекта данных.\r\nИзменение и удаление данных: Пользователь вправе изменить или удалить предоставленные персональные данные, обратившись к администратору приложения.\r\nЗаключительные положения: Настоящее соглашение является публичным и доступно для ознакомления любому лицу, пользующемуся приложением.\r\nЗаключительные положения\r\nПользователь, регистрируясь и используя приложение для бронирования гостиницы, подтверждает свое согласие с условиями обработки персональных данных, изложенными в настоящем соглашении.");
        }

        private void button_find_room_Click(object sender, EventArgs e)
        {
            string day_in = textBox_date_in_day.Text;
            string month_in = textBox_date_in_month.Text;
            string year_in = textBox_date_in_year.Text;

            string date_in = day_in + month_in + year_in;

            string day_out = textBox_date_out_day.Text;
            string month_out = textBox_date_out_month.Text;
            string year_out = textBox_date_out_year.Text;

            string date_out = day_out + month_out + year_out;

            string num_bed = textBox_num_bed.Text;
            

            bool day_in_dig = date_in.All(char.IsDigit);
            bool day_out_dig = date_out.All(char.IsDigit);
            bool num_bed_dig = num_bed.All(char.IsDigit);
            if (day_in_dig == false || day_out_dig == false || num_bed_dig == false || int.Parse(num_bed) > 5 || int.Parse(num_bed) < 0 ||
                (radioButton_yes.Checked == false && radioButton_no.Checked==false) || int.Parse(day_in) > 30 ||
                int.Parse(month_in) > 12 || int.Parse(year_in) < 2023 || int.Parse(year_in) > 2024 || int.Parse(day_out) > 30 ||
                int.Parse(month_out) < 1 || int.Parse(month_out) > 12 || int.Parse(year_out) < 2023 || int.Parse(year_out) > 2024 ||
                (int.Parse(year_in) == 2023 && int.Parse(month_in) == 12 && int.Parse(day_in) < 20) ||
                int.Parse(year_in) > int.Parse(year_out) ||
                int.Parse(year_in) == int.Parse(year_out) && int.Parse(month_in) > int.Parse(month_out) ||
                int.Parse(year_in) == int.Parse(year_out) && int.Parse(month_in) == int.Parse(month_out) &&
                int.Parse(day_in) >= int.Parse(day_out))
            {
                MessageBox.Show("Форма заполнена неверно.");
            }


            else
            {
                book.check_in = day_in + "." + month_in + "." + year_in;
                book.check_out = day_out + "." + month_out + "." + year_out;
                book.beds = num_bed;
                if(radioButton_no.Checked ==true)
                {
                    book.breakfast = "Нет";
                }
                else
                {
                    book.breakfast = "Да";
                }

                NpgsqlConnection conn = GetConnection();
                NpgsqlCommand cmd = conn.CreateCommand();
                NpgsqlDataReader reader;

                try
                {

                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {

                        cmd.CommandText = $"SELECT EXISTS (SELECT * from \"book\" WHERE check_in = '{book.check_in}')";
                        reader = cmd.ExecuteReader();

                        if (reader.Read() && reader.GetBoolean(0) == true)
                        {
                            MessageBox.Show("Данная дата заезда уже занята!");
                            return;
                        }
                        reader.Close();

                        cmd.CommandText = $"SELECT EXISTS (SELECT * from \"book\" WHERE check_out = '{book.check_out}')";
                        reader = cmd.ExecuteReader();

                        if (reader.Read() && reader.GetBoolean(0) == true)
                        {
                            MessageBox.Show("Данная дата выезда уже занята!");
                            return;
                        }
                        reader.Close();

                        MessageBox.Show($"Форма заполнена верно");

                        tabControl1.SelectedIndex = 4;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

           



        }

        private void button_next1_Click(object sender, EventArgs e)
        {
            string br = string.Empty;
            string trans = string.Empty;
            string clean = string.Empty;
            string bkz = string.Empty;
            string child = string.Empty;
            string spa = string.Empty;

            int br_p = 20;
            int trans_p = 15;
            int clean_p = 36;
            int bkz_p = 245;
            int child_p = 10;
            int spa_p = 26;

            int price_for_extra = 0;

            if (checkBox_br.Checked == true)
            {
                br = "Завтрак в номер, ";
                price_for_extra += br_p;
            }
            if(checkBox_trans.Checked == true)
            {
                trans = "Трансфер от/до аэропорта, ";
                price_for_extra += trans_p;
            }
            if(checkBox_clean.Checked == true)
            {
                clean = "Клининг в номер, ";
                price_for_extra += clean_p;
            }
            if(checkBox_bkz.Checked == true)
            {
                bkz = "Бизнес-/Конференц-зал, ";
                price_for_extra += bkz_p;
            }
            if(checkBox_child.Checked == true)
            {
                child = "Детские площадки, ";
                price_for_extra += child_p;
            }
            if(checkBox_spa.Checked == true)
            {
                spa = "Спа/Бассейн";
                price_for_extra += spa_p;
            }

            string ex = br + trans + clean + bkz + child + spa;
            book.extra = ex;

            tabControl1.SelectedIndex = 5;

            textBox_date_in2.Text = book.check_in;
            textBox_date_out2.Text = book.check_out;
            textBox_num_bed2.Text = book.beds;
            textBox_breakfast2.Text = book.breakfast;

            int price_for_res = 0;

            if(int.Parse(book.beds) == 1)
            {
                price_for_res += 500;
            }
            else if (int.Parse(book.beds) == 2)
            {
                price_for_res += 1000;
            }
            else if (int.Parse(book.beds) == 3)
            {
                price_for_res += 1200;
            }
            else if (int.Parse(book.beds) == 4)
            {
                price_for_res += 1500;
            }

            textBox_booking_price.Text = price_for_res.ToString() + "$";
            textBox_extra_serv.Text = book.extra;

            int price_p = price_for_res + price_for_extra;
            book.price = price_p.ToString() + "$";
            textBox_sum_end.Text = book.price;

            label_do_pay.Visible = false;
            label_dont_pay.Visible = true;
            button_price_end.Visible = true;
            button_back5.Visible = false;
            button_back2.Visible = true;
        }

        private void button_back1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button_back2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void button_finich_payment_Click(object sender, EventArgs e)
        {
            string num_card = textBox_num_card.Text;
            string CVC = textBox_CVC_card.Text;
            if(num_card.All(char.IsDigit)==false && CVC.All(char.IsDigit)==false && num_card.Length!=16 && CVC.Length!=3)
            {
                MessageBox.Show("Данные введены неверно!");
                
              
            }

            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();
            NpgsqlDataReader reader;

            try
            {

                conn.Open();

                if (conn.State == ConnectionState.Open)
                {

                    cmd.CommandText = $"INSERT INTO \"book\"(check_in, check_out, beds, breakfast, price, extra) values ('{book.check_in}', '{book.check_out}', '{book.beds}', '{book.breakfast}', '{book.price}', '{book.extra}')";
                    reader = cmd.ExecuteReader();

                    MessageBox.Show($"Оплата прошла успешно!");

                    tabControl1.SelectedIndex = 3;
                    label_dont_pay.Visible = false;
                    label_do_pay.Visible = true;
                    CLear_all();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        private void CLear_all()
        {
            textBox_date_in_day.Clear();
            textBox_date_in_month.Clear();
            textBox_date_in_year.Clear();
            textBox_date_out_day.Clear();
            textBox_date_out_month.Clear();
            textBox_date_out_year.Clear();
            textBox_num_bed.Clear();
            radioButton_no.Checked = false;
            radioButton_yes.Checked = false;

            checkBox_br.Checked = false;
            checkBox_trans.Checked = false;
            checkBox_spa.Checked = false;
            checkBox_clean.Checked = false;
            checkBox_child.Checked = false;
            checkBox_bkz.Checked = false;

            textBox_CVC_card.Clear();
            textBox_num_card.Clear();

        }

        private void button_back3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button_order_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
            button_back5.Visible = true;
            button_back2.Visible = false;
        }

        private void button_price_end_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
            button_price_end.Visible = false;
            button_back2.Visible = false;
            button_back2.Visible = true;
        }

        private void button_back5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button_serveces_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
        }

        private void button_back4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button_feedback_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 8;
        }

        private void pictureBox_open_1_Click(object sender, EventArgs e)
        {
            pictureBox_open_1.Visible = false;
            pictureBox_clothe_1.Visible = true;
            textBox_reg_password.UseSystemPasswordChar = false;
        }

        private void pictureBox_clothe_1_Click(object sender, EventArgs e)
        {
            pictureBox_open_1.Visible = true;
            pictureBox_clothe_1.Visible = false;
            textBox_reg_password.UseSystemPasswordChar = true;
        }

        private void pictureBox_open_2_Click(object sender, EventArgs e)
        {
            pictureBox_open_2.Visible = false;
            pictureBox_clothe_2.Visible = true;
            textBox_reg_password2.UseSystemPasswordChar = false;
        }

        private void pictureBox_clothe_2_Click(object sender, EventArgs e)
        {
            pictureBox_open_2.Visible = true;
            pictureBox_clothe_2.Visible = false;
            textBox_reg_password2.UseSystemPasswordChar = true;
        }

        private void pictureBox_open_3_Click(object sender, EventArgs e)
        {
            pictureBox_open_3.Visible = false;
            pictureBox_clothe_3.Visible = true;
            textBox_password_enter.UseSystemPasswordChar = false;
        }

        private void pictureBox_clothe_3_Click(object sender, EventArgs e)
        {
            pictureBox_open_3.Visible = true;
            pictureBox_clothe_3.Visible = false;
            textBox_password_enter.UseSystemPasswordChar = true;
        }

        private void button_back6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button_get_feedback_Click(object sender, EventArgs e)
        {
            bool score = textBox_score.Text.All(char.IsDigit);
            if(score == false | int.Parse(textBox_score.Text) >10)
            {
                MessageBox.Show("Форма заполнена неверно!");
            }

            else
            {
                book.score = textBox_score.Text;
                book.feedback = textBox_feedback.Text;

                NpgsqlConnection conn = GetConnection();
                NpgsqlCommand cmd = conn.CreateCommand();
                NpgsqlDataReader reader;

                try
                {

                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {

                        cmd.CommandText = $"INSERT INTO \"book\"(feedback, score) values ('{book.feedback}', '{book.score}')";
                        reader = cmd.ExecuteReader();

                        MessageBox.Show($"Отзыв успешно сохранен!");

                        linkLabel_name.Visible = true;
                        label_score.Visible = true;
                        label_feedback.Visible = true;
                        label59.Visible = true;
                        pictureBox_person.Visible = true;

                        linkLabel_name.Text = user.name;
                        label_score.Text = book.score;
                        label_feedback.Text = book.feedback;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
