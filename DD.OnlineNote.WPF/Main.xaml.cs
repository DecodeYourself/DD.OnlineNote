using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DD.OnlineNote.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace DD.OnlineNote.WPF
{
    public partial class Main : Window
    {
        Guid owner;
        Guid selectedNote;
        ServiceProvider provider;
        Category newCategory;
        IEnumerable<Note> notes;

        public IEnumerable<Note> Notes { get
            {
                return notes;
            }
            set
            {
                
                notes = value;
                if(UsrNotes != null)
                    UsrNotes.SetTitles(notes);
            }
        }
        public UserNotes UsrNotes { get; set; }

        public Guid SelectedNote {
            get
            {
                return selectedNote;
            }
            set
            {
                selectedNote = value;

                if (UsrNotes != null)
                    UsrNotes.Setter(Notes.Where(x => x.Id == selectedNote).FirstOrDefault());
                else
                    UsrNotes = Notes.Select(x => new UserNotes(x)).FirstOrDefault();

                ReturnFocuseToNote();
            }
        }

        public Main(Guid usrOwner)
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;
            provider = ServiceProvider.GetProvider();
            owner = usrOwner;
            newCategory = new Category();

        }

        private async Task FillNoteList()
        {
            await NotesUpd();
            UsrNotes = Notes.Select(x => new UserNotes(x)).FirstOrDefault();
            if (UsrNotes == null)
                UsrNotes = new UserNotes(null);
            DataContext = UsrNotes;
            UsrNotes.SetTitles(Notes);
            NoteListBox.SelectedIndex = NoteListBox.Items.Count > 0 ? NoteListBox.Items.Count - 1 : -1;
            await ReturnFocuseToCategory();
        }

        private async Task NotesUpd()
        {
            Notes = await provider.GetUserNotes(owner);
        }
        private async Task ReturnFocuseToCategory()
        {
            if (SelectedNote != default(Guid))
            {
                Guid categoryName = await provider.GetCategoryNameByNoteId(SelectedNote);

                Category returnFocus = UsrNotes.NoteCategories.Where(x => x.Id == categoryName).FirstOrDefault();
                CagegoryList.SelectedIndex = returnFocus == null ? -1 : UsrNotes.NoteCategories.LastIndexOf(returnFocus);
            }
            else
            {
                CagegoryList.SelectedIndex = 0;
            }

        }
        private void ReturnFocuseToNote()
        {
            NoteListBox.SelectedIndex = (NoteListBox.Items.SourceCollection as Dictionary<Guid, string>).Select(x => x.Key).ToList().IndexOf(SelectedNote);

        }
        private async void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await FillNoteList();


        }

        private async Task RefreshNotes()
        {
            await NotesUpd();

            if (UsrNotes != null)
            {
                UsrNotes.Setter(Notes.FirstOrDefault());
                UsrNotes.SetTitles(Notes);
            }
            else
                UsrNotes = Notes.Select(x => new UserNotes(x)).FirstOrDefault();

            await ReturnFocuseToCategory();

           
            
        }
        private async Task FillSelectedNotes(Guid NoteId)
        {
            Note slctedNote = Notes.Where(x => x.Id == NoteId).FirstOrDefault();
            if (slctedNote == null)
            {
                await NotesUpd();
                slctedNote = Notes.Where(x => x.Id == NoteId).FirstOrDefault();

                if (slctedNote == null)
                    return;

                UsrNotes.Setter(slctedNote);
            }
            else
                UsrNotes.Setter(slctedNote);

            UsrNotes.SetTitles(Notes);
            await ReturnFocuseToCategory();
        }

        private void LogoffBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow lgw = new LoginWindow();
            lgw.Show();
            this.Close();
        }

        private async void NoteCreate_Click(object sender, RoutedEventArgs e)
        {
            Note newNote = new Note()
            {
                Title = $"New note",
                Content = $"Empty content",
                Owner = new User() { Id = owner },
                DateCreated = DateTime.Now,
                Categories = await provider.GetUserCategories(owner),
                SharedNote = null
            };
            newNote = await provider.CreateNote(newNote);
            SelectedNote = newNote.Id;
            await RefreshNotes();
            ReturnFocuseToNote();

        }

        private async void NoteDelete_Click(object sender, RoutedEventArgs e)
        {
            await provider.DeleteNote(SelectedNote);
            await NotesUpd();
            UsrNotes.Setter(Notes.Select(x => x).FirstOrDefault());
            UsrNotes.SetTitles(Notes);
            ReturnFocuseToNote();
        }

        private void CagegoryList_TextChanged(object sender, RoutedEventArgs e)
        {
            if (newCategory.Id != default(Guid))
                newCategory.Name = (e.OriginalSource as TextBox).Text;
        }

        private void CagegoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.RemovedItems.Count != 0 && e.AddedItems.Count != 0) // Выбор другой категории
                newCategory.Id = (e.AddedItems[0] as Category).Id;
            else if (e.RemovedItems.Count != 0 && (e.Source as ComboBox).SelectedIndex == -1) // изменения текста в текущий категории
                newCategory.Id = (e.RemovedItems[0] as Category).Id;
            else if (e.RemovedItems.Count != 0 && (e.Source as ComboBox).SelectedIndex != -1) // удаление текущей
                newCategory.Id = (e.RemovedItems[0] as Category).Id;
            else if(e.AddedItems.Count != 0 && (e.Source as ComboBox).SelectedIndex != -1)// && newCategory.Id == default(Guid)) // selectedIndex установлен из кода
                newCategory.Id = (e.AddedItems[0] as Category).Id;
            

            //if (newCategory.Id != default(Guid) && (e.Source as ComboBox).SelectedIndex != -1) // 
            //    newCategory = new Category();

        }

        private async void CategoryAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(newCategory.Name) && newCategory.Id != default(Guid))
            {
                Category returnFocus = UsrNotes.NoteCategories.Where(x => x.Id == newCategory.Id).FirstOrDefault();
                CagegoryList.SelectedIndex = returnFocus == null ? -1 : UsrNotes.NoteCategories.LastIndexOf(returnFocus);
                return;
            }

            Category currentCat;
            if (newCategory.Id == default(Guid) ||
                UsrNotes.NoteCategories.Where(x => x.Id == newCategory.Id).FirstOrDefault()?.Name == newCategory.Name) //Не изменяя текста нажали Save
            {
                currentCat = await provider.CreateCategory(owner, "New Category");
            }
            else{
                currentCat = await provider.UpdateCategory(newCategory);
            }
            UsrNotes.UpdateCurrentCategoryList(await provider.GetUserCategories(owner));

            Category focusedCategory = UsrNotes.NoteCategories.Where(x => x.Id == currentCat.Id).FirstOrDefault();
            CagegoryList.SelectedIndex = focusedCategory == null ? -1 : UsrNotes.NoteCategories.LastIndexOf(focusedCategory);

            newCategory = new Category();

        }

        private async void CategoryDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(newCategory.Name) && newCategory.Id == default(Guid) && CagegoryList.SelectedIndex == -1)
            {
                Category returnFocus = UsrNotes.NoteCategories.Where(x => x.Id == newCategory.Id).FirstOrDefault();
                CagegoryList.SelectedIndex = returnFocus == null ? -1 : UsrNotes.NoteCategories.LastIndexOf(returnFocus);
                return;
            }
            Guid selectedCategory = UsrNotes.NoteCategories[CagegoryList.SelectedIndex].Id;
            

            await provider.DeleteCategory(selectedCategory);
            UsrNotes.UpdateCurrentCategoryList(await provider.GetUserCategories(owner));

            CagegoryList.SelectedIndex = 0;

            newCategory = new Category();
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Category returnFocus;
            if (string.IsNullOrEmpty(newCategory.Name) && newCategory.Id == default(Guid) && CagegoryList.SelectedIndex == -1)
            {
                returnFocus = UsrNotes.NoteCategories.Where(x => x.Id == newCategory.Id).FirstOrDefault();
                CagegoryList.SelectedIndex = returnFocus == null ? -1 : UsrNotes.NoteCategories.LastIndexOf(returnFocus);
            }
            if (CagegoryList.SelectedIndex == -1)
                return;

            Guid selectedCategory = UsrNotes.NoteCategories[CagegoryList.SelectedIndex].Id;
            IEnumerable<Category> _category = ((IEnumerable<Category>)UsrNotes.NoteCategories).Where(x => x.Id == selectedCategory);
            if (_category == null)
                return;

            Note savedNote = new Note
            {
                Id = UsrNotes.NoteId,
                Content = UsrNotes.NoteContent,
                Title = UsrNotes.NoteTitle,
                DateChanged = DateTime.Now,
                Categories = _category
            };
            savedNote = await provider.UpdateNote(savedNote);
            selectedNote = savedNote.Id;
            UsrNotes.Setter(savedNote);
            await NotesUpd();
            UsrNotes.SetTitles(Notes);
            returnFocus = UsrNotes.NoteCategories.Where(x => x.Id == selectedCategory).FirstOrDefault();
            CagegoryList.SelectedIndex = returnFocus == null ? -1 : UsrNotes.NoteCategories.LastIndexOf(returnFocus);
        }

        private async void NoteListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((e.Source as ListBox).SelectedIndex == -1)
                return;
            if(e.AddedItems.Count != 0 && (e.Source as ListBox).SelectedIndex != -1)
            {
                SelectedNote = ((KeyValuePair<Guid, string>)((object[])e.AddedItems)[0]).Key;

                await FillSelectedNotes(SelectedNote);
            }
            //if (e.RemovedItems.Count != 0 && e.AddedItems.Count != 0) // Выбор другой категории
            //    newCategory.Id = (e.AddedItems[0] as Category).Id;
            //else if (e.RemovedItems.Count != 0 && (e.Source as ListBox).SelectedIndex == -1) // изменения текста в текущий категории
            //    newCategory.Id = (e.RemovedItems[0] as Category).Id;
            //else if (e.RemovedItems.Count != 0 && (e.Source as ListBox).SelectedIndex != -1) // удаление текущей
            //    newCategory.Id = (e.RemovedItems[0] as Category).Id;
            //else if (e.AddedItems.Count != 0 && (e.Source as ListBox).SelectedIndex != -1)// && newCategory.Id == default(Guid)) // selectedIndex установлен из кода
            //    newCategory.Id = (e.AddedItems[0] as Category).Id;
        }
    }
    public class UserNotes : INotifyPropertyChanged
    {
        Dictionary<Guid,string> _Titles;
        string _NoteTitle;
        string _NoteContent;
        DateTime _NoteDateCreated;
        DateTime _NoteDateChanged;
        List<Category> _NoteCategories;
        List<User> _NoteSharedNote;
        public Guid NoteId { get; set; }

        public Dictionary<Guid, string> Titles {
            get
            {
                return _Titles;
            }
            set
            {
                _Titles = value;
                OnPropertyChanged("Titles");
            }
            }
        public string NoteTitle
        {
            get
            {
                return _NoteTitle;
            }
            set
            {
                _NoteTitle = value;
                OnPropertyChanged("NoteTitle");
            }
        }
        public string NoteContent
        {
            get
            {
                return _NoteContent;
            }
            set
            {
                _NoteContent = value;
                OnPropertyChanged("NoteContent");
            }
        }
        public DateTime NoteDateCreated
        {
            get
            {
                return _NoteDateCreated;
            }
            set
            {
                _NoteDateCreated = value;
                OnPropertyChanged("NoteDateCreated");
            }
        }
        public DateTime NoteDateChanged
        {
            get
            {
                return _NoteDateChanged;
            }
            set
            {
                _NoteDateChanged = value;
                OnPropertyChanged("NoteDateChanged");
            }
        }
        public List<Category> NoteCategories
        {
            get
            {
                return _NoteCategories;
            }
            set
            {
                _NoteCategories = value;
                OnPropertyChanged("NoteCategories");
            }
        }
        public List<User> NoteSharedNote
        {
            get
            {
                return _NoteSharedNote;
            }
            set
            {
                _NoteSharedNote = value;
                OnPropertyChanged("NoteSharedNote");
            }
        }

        public UserNotes(Note currentNote)
        {
            if (currentNote == null)
                return;
            NoteId = currentNote.Id;
            NoteTitle = currentNote.Title;
            NoteContent = currentNote.Content;
            NoteDateCreated = currentNote.DateCreated;
            NoteDateChanged = currentNote.DateChanged;
            NoteCategories = currentNote.Categories.ToList();
            NoteSharedNote = currentNote.SharedNote.ToList();
        }
        public void Setter(Note currentNote)
        {
            if (currentNote == null)
                return;
            NoteId = currentNote.Id;
            NoteTitle = currentNote.Title;
            NoteContent = currentNote.Content;
            NoteDateCreated = currentNote.DateCreated;
            NoteDateChanged = currentNote.DateChanged;
            NoteCategories = currentNote.Categories.ToList();
            NoteSharedNote = currentNote.SharedNote.ToList();


    }
    public void UpdateCurrentCategoryList(IEnumerable<Category> CategoryList)
        {
            NoteCategories = CategoryList.ToList();
        }
        public void SetTitles(IEnumerable<Note> Notes)
        {
            
            //foreach (Note item in Notes)
            //{
            //    Titles.Add(item.Id, item.Title);
            //}
            Titles = Notes.ToDictionary(x => x.Id,x=> x.Title);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
   
}
