﻿using Common;
using Services.MyService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Unity;

namespace Module.Catalog
{
    public class CatalogViewModel : INotifyPropertyChanged
    {
        #region Fields

        private IUnityContainer _container;
        private CatalogOrder _model;
        private IPersonService _service;
        private List<Person> _fullPeopleList;

        private List<Person> _catalog;
        private DateTime _lastUpdateTime;
        private bool _include70s;
        private bool _include80s;
        private bool _include90s;
        private bool _include00s;

        #endregion

        #region Properties

        public CatalogOrder Model
        {
            get { return _model; }
            set
            {
                if (_model == value)
                    return;
                _model = value;
                RaisePropertyChanged("Model");
            }
        }

        public List<Person> Catalog
        {
            get { return _catalog; }
            set
            {
                if (_catalog == value)
                    return;
                _catalog = value;
                RaisePropertyChanged("Catalog");
            }
        }

        public DateTime LastUpdateTime
        {
            get { return _lastUpdateTime; }
            set
            {
                if (_lastUpdateTime == value)
                    return;
                _lastUpdateTime = value;
                RaisePropertyChanged("LastUpdateTime");
            }
        }

        public bool Include70s
        {
            get { return _include70s; }
            set
            {
                if (_include70s == value)
                    return;
                _include70s = value;
                RefreshFilter();
            }
        }

        public bool Include80s
        {
            get { return _include80s; }
            set
            {
                if (_include80s == value)
                    return;
                _include80s = value;
                RefreshFilter();
            }
        }

        public bool Include90s
        {
            get { return _include90s; }
            set
            {
                if (_include90s == value)
                    return;
                _include90s = value;
                RefreshFilter();
            }
        }

        public bool Include00s
        {
            get { return _include00s; }
            set
            {
                if (_include00s == value)
                    return;
                _include00s = value;
                RefreshFilter();
            }
        }

        private bool IsCacheValid
        {
            get
            {
                return DateTime.Now - LastUpdateTime < TimeSpan.FromSeconds(10);
            }
        }

        #endregion

        #region Constructors

        public CatalogViewModel(IUnityContainer container)
        {
            _container = container;
        }

        #endregion

        #region Methods

        public void Initialize()
        {
            _service = GetServiceFromContainer();
            _model = GetModelFromContainer();
            RefreshCatalog();
        }

        private CatalogOrder GetModelFromContainer()
        {
            if (!_container.IsRegistered<CatalogOrder>("CurrentOrder"))
                throw new MissingFieldException(
                    "CurrentOrder is not available from the DI Container");
            return _container.Resolve<CatalogOrder>("CurrentOrder");
        }

        private IPersonService GetServiceFromContainer()
        {
            if (!_container.IsRegistered<IPersonService>())
                throw new MissingFieldException(
                    "IPersonService is not available from the DI Container");
            return _container.Resolve<IPersonService>();
        }

        public void RefreshCatalog()
        {
            if (IsCacheValid)
            {
                ResetFiltersToDefaults();
            }
            else
            {
                RefreshCatalogFromService();
            }
        }

        private void RefreshCatalogFromService()
        {
            Catalog = new List<Person>();

            var asyncBegin = _service.BeginGetPeople(null, null);
            var task = Task<List<Person>>.Factory.FromAsync(
                asyncBegin, _service.EndGetPeople);
            task.ContinueWith(t =>
            {
                _fullPeopleList = t.Result;
                ResetFiltersToDefaults();
                LastUpdateTime = DateTime.Now;
            }, TaskContinuationOptions.NotOnFaulted);

            HandleServiceException(task);
        }

        private static void HandleServiceException(Task<List<Person>> task)
        {
            Action<Exception> rtxDel = (ex) => { throw ex; };
            var uiDispatcher = Dispatcher.CurrentDispatcher;
            task.ContinueWith(rp =>
            {
                uiDispatcher.Invoke(rtxDel, rp.Exception.InnerException);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private void ResetFiltersToDefaults()
        {
            _include70s = true;
            _include80s = true;
            _include90s = true;
            _include00s = true;
            RefreshFilter();
        }

        public void AddToSelection(object person)
        {
            var selectedPerson = person as Person;
            if (selectedPerson == null)
                return;

            if (!Model.SelectedPeople.Contains(selectedPerson))
            {
                Model.SelectedPeople.Add(selectedPerson);
            }
        }

        public void RemoveFromSelection(object person)
        {
            var selectedPerson = person as Person;
            if (selectedPerson == null)
                return;

            if (Model.SelectedPeople.Contains(selectedPerson))
            {
                Model.SelectedPeople.Remove(selectedPerson);
            }
        }

        public void ClearSelection()
        {
            Model.SelectedPeople.Clear();
        }

        private void RefreshFilter()
        {
            RaisePropertyChanged("Include70s");
            RaisePropertyChanged("Include80s");
            RaisePropertyChanged("Include90s");
            RaisePropertyChanged("Include00s");

            IEnumerable<Person> people = _fullPeopleList;
            if (!Include70s)
                people = people.Where(p => p.StartDecade != 1970);
            if (!Include80s)
                people = people.Where(p => p.StartDecade != 1980);
            if (!Include90s)
                people = people.Where(p => p.StartDecade != 1990);
            if (!Include00s)
                people = people.Where(p => p.StartDecade != 2000);

            Catalog = people.ToList();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
