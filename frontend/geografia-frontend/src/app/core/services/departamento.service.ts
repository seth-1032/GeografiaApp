
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
    Departamento,
    GuardarDepartamento
} from '../models/departamento.model';

@Injectable({
    providedIn: 'root'
})
export class DepartamentoService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = 'http://localhost:5088/api/departamentos';

    obtenerTodos(): Observable<Departamento[]> {
        return this.http.get<Departamento[]>(this.apiUrl);
    }

    obtenerPorPais(paisId: number): Observable<Departamento[]> {
        return this.http.get<Departamento[]>(
            `${this.apiUrl}/por-pais/${paisId}`
        );
    }

    obtenerPorId(id: number): Observable<Departamento> {
        return this.http.get<Departamento>(
            `${this.apiUrl}/${id}`
        );
    }

    crear(datos: GuardarDepartamento): Observable<Departamento> {
        return this.http.post<Departamento>(
            this.apiUrl,
            datos
        );
    }

    actualizar(
        id: number,
        datos: GuardarDepartamento
    ): Observable<Departamento> {
        return this.http.put<Departamento>(
            `${this.apiUrl}/${id}`,
            datos
        );
    }

    eliminar(id: number): Observable<void> {
        return this.http.delete<void>(
            `${this.apiUrl}/${id}`
        );
    }
}
