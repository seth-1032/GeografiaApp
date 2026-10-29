
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pais, GuardarPais } from '../models/pais.model';

@Injectable({
  providedIn: 'root'
})
export class PaisService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5088/api/paises';

  obtenerTodos(): Observable<Pais[]> {
    return this.http.get<Pais[]>(this.apiUrl);
  }

  obtenerPorId(id: number): Observable<Pais> {
    return this.http.get<Pais>(`${this.apiUrl}/${id}`);
  }

  crear(datos: GuardarPais): Observable<Pais> {
    return this.http.post<Pais>(this.apiUrl, datos);
  }

  actualizar(id: number, datos: GuardarPais): Observable<Pais> {
    return this.http.put<Pais>(`${this.apiUrl}/${id}`, datos);
  }

  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}