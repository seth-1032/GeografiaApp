
import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'inicio',
        pathMatch: 'full'
    },
    {
        path: 'inicio',
        loadComponent: () =>
            import('./features/inicio/inicio')
                .then(m => m.Inicio)
    },
    {
        path: 'paises',
        loadComponent: () =>
            import('./features/paises/paises')
                .then(m => m.Paises)
    },
    {
        path: 'departamentos',
        loadComponent: () =>
            import('./features/departamentos/departamentos')
                .then(m => m.Departamentos)
    },
    {
        path: 'ciudades',
        loadComponent: () =>
            import('./features/ciudades/ciudades')
                .then(m => m.Ciudades)
    },
    {
        path: '**',
        redirectTo: 'inicio'
    }
];
