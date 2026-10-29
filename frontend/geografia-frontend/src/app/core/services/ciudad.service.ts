
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Ciudad, GuardarCiudad } from '../models/ciudad.model';

@Injectable({
    providedIn: 'root'
})
export class CiudadService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = 'http://localhost:5088/api/ciudades';

    obtenerTodos(): Observable<Ciudad[]> {
        return this.http.get<Ciudad[]>(this.apiUrl);
    }

    obtenerPorId(id: number): Observable<Ciudad> {
        return this.http.get<Ciudad>(`${this.apiUrl}/${id}`);
    }

    crear(datos: GuardarCiudad): Observable<Ciudad> {
        return this.http.post<Ciudad>(this.apiUrl, datos);
    }

    actualizar(id: number, datos: GuardarCiudad): Observable<Ciudad> {
        return this.http.put<Ciudad>(`${this.apiUrl}/${id}`, datos);
    }

    eliminar(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
