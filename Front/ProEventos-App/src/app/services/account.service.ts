import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/models/identity/User';
import { UserUpdate } from '@app/models/identity/userUpdate';
import { Observable, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private currentUserSource = new ReplaySubject<User>(1);
  public currentUser$ = this.currentUserSource.asObservable(); // Cifrão indica que é um obserble

  public baseURL = environment.apiURL + 'api/account/';

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void>
  {
    return this.http.post<User>(this.baseURL + 'login', model)
                  .pipe(
                      take(1),
                      map((response: User) => {
                        const user = response;
                        if(user)
                        {
                          this.setCurrentUser(user);
                        }
                      })
                  );
  }

  public register(user: User): Observable<void>
  {
    return this.http.post<User>(`${this.baseURL}register`, user)
              .pipe(
                  take(1),
                  map((response: User) => {
                    const user = response;
                    if(user)
                    {
                      this.setCurrentUser(user);
                    }
                  })
              );
  }

  public logout(): void
  {
    localStorage.removeItem('user');
    this.currentUserSource.next(null!);
    // this.currentUserSource.complete();
  }

  public setCurrentUser(user: User): void
  {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
    console.log(this.currentUser$);
  }

  public getUser(): Observable<UserUpdate>
  {
    return this.http.get<UserUpdate>(this.baseURL + 'getUser')
                    .pipe(take(1));
  }

  public updateUser(user: UserUpdate): Observable<void>
  {
    return this.http.put<UserUpdate>(this.baseURL + 'UpdateUser', user)
                    .pipe(
                      take(1),
                      map((response: UserUpdate) => {
                        this.setCurrentUser(response);
                      }
            )
      );
  }

  public uploadImage(file: File[]): Observable<UserUpdate>
  {
    const fileToUpload = file[0] as File;

    const formData = new FormData();

    formData.append('file', fileToUpload);

    return this.http.post<UserUpdate>(this.baseURL + 'upload-image',  formData).pipe(take(1));
  }
}
